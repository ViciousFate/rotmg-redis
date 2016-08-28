using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.realm;
using common;
using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    class GotoAckPacketHandler : PacketHandlerBase<GotoAckPacket>
    {
        public override PacketID ID { get { return PacketID.GotoAck; } }

        protected override void HandlePacket(Client client, GotoAckPacket packet)
        {
            //TODO: implement something
        }
    }
}
