using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.realm;
using common;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.commands;

namespace wServer.networking.handlers
{
    class PlayerTextPacketHandler : PacketHandlerBase<PlayerTextPacket>
    {
        public override PacketID ID { get { return PacketID.PlayerText; } }

        protected override void HandlePacket(Client client, PlayerTextPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet.Text));
        }

        void Handle(Player player, RealmTime time, string text)
        {
            if (player.Owner == null) return;

            if (text[0] == '/')
                player.Manager.Commands.Execute(player, time, text);
            else
                player.Manager.Chat.Say(player, text);
        }
    }
}
