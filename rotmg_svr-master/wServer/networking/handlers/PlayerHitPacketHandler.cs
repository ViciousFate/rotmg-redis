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
    class PlayerHitPacketHandler : PacketHandlerBase<PlayerHitPacket>
    {
        public override PacketID ID { get { return PacketID.PlayerHit; } }

        protected override void HandlePacket(Client client, PlayerHitPacket packet)
        {
            //TODO: implement something
        }
    }
}
