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
    class EscapePacketHandler : PacketHandlerBase<EscapePacket>
    {
        public override PacketID ID { get { return PacketID.Escape; } }

        protected override void HandlePacket(Client client, EscapePacket packet)
        {
            if (client.Player.Owner.Name == "Nexus") client.Player.SendInfo("You cannot nexus in nexus."); return;
            client.Reconnect(new ReconnectPacket()
            {
                Host = "",
                Port = 2050,
                GameId = World.NEXUS_ID,
                Name = "Nexus",
                Key = Empty<byte>.Array,
            });
        }
    }
}
