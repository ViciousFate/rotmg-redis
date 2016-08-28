using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Net;
using wServer.realm;
using common;
using log4net;

namespace wServer.networking
{
    //hackish code
    class NetworkHandler
    {
        static ILog log = LogManager.GetLogger(typeof(NetworkHandler));

        enum ReceiveState
        {
            Awaiting,
            ReceivingHdr,
            ReceivingBody,
            Processing
        }
        class ReceiveToken
        {
            public int Length;
            public PacketID ID;
            public byte[] PacketBody;
        }
        enum SendState
        {
            Awaiting,
            Ready,
            Sending
        }
        class SendToken
        {
            public Packet Packet;
        }

        public const int BUFFER_SIZE = 0x10000;

        SocketAsyncEventArgs receive;
        ReceiveState receiveState = ReceiveState.Awaiting;
        byte[] receiveBuff;

        SocketAsyncEventArgs send;
        SendState sendState = SendState.Awaiting;
        byte[] sendBuff;

        Socket skt;
        Client parent;
        public NetworkHandler(Client parent, Socket skt)
        {
            this.parent = parent;
            this.skt = skt;
        }

        public void BeginHandling()
        {
            skt.NoDelay = true;
            skt.UseOnlyOverlappedIO = true;

            send = new SocketAsyncEventArgs();
            send.Completed += SendCompleted;
            send.UserToken = new SendToken();
            send.SetBuffer(sendBuff = new byte[BUFFER_SIZE], 0, BUFFER_SIZE);

            receive = new SocketAsyncEventArgs();
            receive.Completed += ReceiveCompleted;
            receive.UserToken = new ReceiveToken();
            receive.SetBuffer(receiveBuff = new byte[BUFFER_SIZE], 0, BUFFER_SIZE);

            receiveState = ReceiveState.ReceivingHdr;
            receive.SetBuffer(0, 5);
            if (!skt.ReceiveAsync(receive))
                ReceiveCompleted(this, receive);
        }

        void ProcessPolicyFile()    //WUT.
        {
            var s = new NetworkStream(skt);
            NWriter wtr = new NWriter(s);
            wtr.WriteNullTerminatedString(@"<cross-domain-policy>
     <allow-access-from domain=""*"" to-ports=""*"" />
</cross-domain-policy>");
            wtr.Write((byte)'\r');
            wtr.Write((byte)'\n');
            parent.Disconnect();
        }

        //It is said that ReceiveAsync/SendAsync never returns false unless error
        //So...let's just treat it as always true

        void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {

                if (!skt.Connected)
                {
                    parent.Disconnect();
                    return;
                }


                if (e.SocketError != SocketError.Success)
                    throw new SocketException((int)e.SocketError);

                switch (receiveState)
                {
                    case ReceiveState.ReceivingHdr:
                        if (e.BytesTransferred < 5)
                        {
                            parent.Disconnect();
                            return;
                        }

                        if (e.Buffer[0] == 0x3c && e.Buffer[1] == 0x70 &&
                            e.Buffer[2] == 0x6f && e.Buffer[3] == 0x6c && e.Buffer[4] == 0x69)
                        {
                            ProcessPolicyFile();
                            return;
                        }

                        var len = (e.UserToken as ReceiveToken).Length =
                            IPAddress.NetworkToHostOrder(BitConverter.ToInt32(e.Buffer, 0)) - 5;
                        if (len < 0 || len > BUFFER_SIZE)
                            log.ErrorFormat("Buffer not large enough! (requested size={0})", len);
                        (e.UserToken as ReceiveToken).PacketBody = new byte[len];
                        (e.UserToken as ReceiveToken).ID = (PacketID)e.Buffer[4];

                        receiveState = ReceiveState.ReceivingBody;
                        e.SetBuffer(0, len);
                        skt.ReceiveAsync(e);

                        break;
                    case ReceiveState.ReceivingBody:
                        if (e.BytesTransferred < (e.UserToken as ReceiveToken).Length)
                        {
                            parent.Disconnect();
                            return;
                        }

                        var body = (e.UserToken as ReceiveToken).PacketBody;
                        var id = (e.UserToken as ReceiveToken).ID;
                        Buffer.BlockCopy(e.Buffer, 0, body, 0, body.Length);
                        //pkt.Read(parent, e.Buffer, 0, (e.UserToken as ReceiveToken).Length);

                        receiveState = ReceiveState.Processing;
                        bool cont = OnPacketReceived(id, body);

                        if (cont && skt.Connected)
                        {
                            receiveState = ReceiveState.ReceivingHdr;
                            e.SetBuffer(0, 5);
                            skt.ReceiveAsync(e);
                        }
                        break;
                    default:
                        throw new InvalidOperationException(e.LastOperation.ToString());
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (!skt.Connected) return;

                int len;
                switch (sendState)
                {
                    case SendState.Ready:
                        len = (e.UserToken as SendToken).Packet.Write(parent, sendBuff, 0);

                        sendState = SendState.Sending;
                        e.SetBuffer(0, len);

                        if (!skt.Connected) return;
                        skt.SendAsync(e);
                        break;
                    case SendState.Sending:
                        (e.UserToken as SendToken).Packet = null;

                        if (CanSendPacket(e, true))
                        {
                            len = (e.UserToken as SendToken).Packet.Write(parent, sendBuff, 0);

                            sendState = SendState.Sending;
                            e.SetBuffer(0, len);

                            if (!skt.Connected) return;
                            skt.SendAsync(e);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }


        void OnError(Exception ex)
        {
            log.Error("Socket error.", ex);
            parent.Disconnect();
        }
        bool OnPacketReceived(PacketID id, byte[] pkt)
        {
            if (parent.IsReady())
            {
                parent.Manager.Network.AddPendingPacket(parent, id, pkt);
                return true;
            }
            else
                return false;
        }
        ConcurrentQueue<Packet> pendingPackets = new ConcurrentQueue<Packet>();
        bool CanSendPacket(SocketAsyncEventArgs e, bool ignoreSending)
        {
            lock (sendLock)
            {
                if (sendState == SendState.Ready ||
                    (!ignoreSending && sendState == SendState.Sending))
                    return false;
                Packet packet;
                if (pendingPackets.TryDequeue(out packet))
                {
                    (e.UserToken as SendToken).Packet = packet;
                    sendState = SendState.Ready;
                    return true;
                }
                else
                {
                    sendState = SendState.Awaiting;
                    return false;
                }
            }
        }

        object sendLock = new object();
        public void SendPacket(Packet pkt)
        {
            if (!skt.Connected) return;
            pendingPackets.Enqueue(pkt);
            if (CanSendPacket(send, false))
            {
                var len = (send.UserToken as SendToken).Packet.Write(parent, sendBuff, 0);

                sendState = SendState.Sending;
                send.SetBuffer(sendBuff, 0, len);
                if (!skt.SendAsync(send))
                    SendCompleted(this, send);
            }
        }
        public void SendPackets(IEnumerable<Packet> pkts)
        {
            if (!skt.Connected) return;
            foreach (var i in pkts)
                pendingPackets.Enqueue(i);
            if (CanSendPacket(send, false))
            {
                var len = (send.UserToken as SendToken).Packet.Write(parent, sendBuff, 0);

                sendState = SendState.Sending;
                send.SetBuffer(sendBuff, 0, len);
                if (!skt.SendAsync(send))
                    SendCompleted(this, send);
            }
        }
    }
}
