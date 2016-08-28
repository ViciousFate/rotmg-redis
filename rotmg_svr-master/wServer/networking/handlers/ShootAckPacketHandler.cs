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
    class ShootAckPacketHandler : PacketHandlerBase<ShootAckPacket>
    {
        public override PacketID ID { get { return PacketID.ShootAck; } }

        protected override void HandlePacket(Client client, ShootAckPacket packet)
        {
            //TODO: implement something
        }
    }
}
