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
    class PongPacketHandler : PacketHandlerBase<PongPacket>
    {
        public override PacketID ID { get { return PacketID.Pong; } }

        protected override void HandlePacket(Client client, PongPacket packet)
        {
            client.Player.Pong(packet.Time);
        }
    }
}
