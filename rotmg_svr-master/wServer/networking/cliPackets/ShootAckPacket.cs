using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;

namespace wServer.networking.cliPackets
{
    public class ShootAckPacket : ClientPacket
    {
        public int Time { get; set; }

        public override PacketID ID { get { return PacketID.ShootAck; } }
        public override Packet CreateInstance() { return new ShootAckPacket(); }

        protected override void Read(NReader rdr)
        {
            Time = rdr.ReadInt32();
        }
        protected override void Write(NWriter wtr)
        {
            wtr.Write(Time);
        }
    }
}
