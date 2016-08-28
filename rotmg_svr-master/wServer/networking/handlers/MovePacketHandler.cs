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
    class MovePacketHandler : PacketHandlerBase<MovePacket>
    {
        public override PacketID ID { get { return PacketID.Move; } }

        protected override void HandlePacket(Client client, MovePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, packet));
        }

        void Handle(Player player, MovePacket packet)
        {
            if (player.Owner == null) return;

            player.Flush();
            if (packet.Position.X == -1 || packet.Position.Y == -1) return;

            double newX = player.X; double newY = player.Y;
            if (newX != packet.Position.X)
            {
                newX = packet.Position.X;
                player.UpdateCount++;
            }
            if (newY != packet.Position.Y)
            {
                newY = packet.Position.Y;
                player.UpdateCount++;
            }
            player.Move((float)newX, (float)newY);
        }
    }
}
