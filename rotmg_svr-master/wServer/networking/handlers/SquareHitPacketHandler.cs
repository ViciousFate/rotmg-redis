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
    class SquareHitPacketHandler : PacketHandlerBase<SquareHitPacket>
    {
        public override PacketID ID { get { return PacketID.SquareHit; } }

        protected override void HandlePacket(Client client, SquareHitPacket packet)
        {
            //TODO: implement something
        }
    }
}
