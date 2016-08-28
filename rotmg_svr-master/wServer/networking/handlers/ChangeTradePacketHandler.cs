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
    class ChangeTradePacketHandler : PacketHandlerBase<ChangeTradePacket>
    {
        public override PacketID ID { get { return PacketID.ChangeTrade; } }

        protected override void HandlePacket(Client client, ChangeTradePacket packet)
        {
            var player = client.Player;
            player.tradeAccepted = false;
            player.tradeTarget.tradeAccepted = false;
            player.trade = packet.Offers;

            player.tradeTarget.Client.SendPacket(new TradeChangedPacket()
            {
                Offers = player.trade
            });
        }
    }
}
