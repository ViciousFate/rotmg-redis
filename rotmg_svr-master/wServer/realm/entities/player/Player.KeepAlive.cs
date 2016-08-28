using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.svrPackets;
using wServer.networking.cliPackets;

namespace wServer.realm.entities
{
    public partial class Player
    {
        long lastPong = -1;
        int? lastTime = null;
        long tickMapping = 0;
        Queue<long> ts = new Queue<long>();

        const int PING_PERIOD = 5000;
        const int DC_THRESOLD = 10000;

        bool sentPing = false;
        bool KeepAlive(RealmTime time)
        {
            if (lastPong == -1) lastPong = time.tickTimes - PING_PERIOD;
            if (time.tickTimes - lastPong > PING_PERIOD && !sentPing)
            {
                sentPing = true;
                ts.Enqueue(time.tickTimes);
                client.SendPacket(new PingPacket());
            }
            else if (time.tickTimes - lastPong > DC_THRESOLD)
            {
                //client.Disconnect();
                return false;
            }
            return true;
        }
        internal void Pong(int time)
        {
            if (lastTime != null && (time - lastTime.Value > DC_THRESOLD || time - lastTime.Value < 0))
                ;//client.Disconnect();
            else
                lastTime = time;
            tickMapping = ts.Dequeue() - time;
            lastPong = time + tickMapping;
            sentPing = false;
            if (!Manager.Database.RenewLock(client.Account))
                client.Disconnect();
        }
    }
}
