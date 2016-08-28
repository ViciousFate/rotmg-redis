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
    class CheckCreditsPacketHandler : PacketHandlerBase<CheckCreditsPacket>
    {
        public override PacketID ID { get { return PacketID.CheckCredits; } }

        protected override void HandlePacket(Client client, CheckCreditsPacket packet)
        {
            client.Account.Flush();
            client.Account.Reload();
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player));
        }

        void Handle(Player player)
        {
            player.Credits = player.Client.Account.Credits;
            player.UpdateCount++;
        }
    }
}
