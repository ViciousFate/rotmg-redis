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
    class AcceptTradePacketHandler : PacketHandlerBase<AcceptTradePacket>
    {
        public override PacketID ID { get { return PacketID.AcceptTrade; } }

        protected override void HandlePacket(Client client, AcceptTradePacket packet)
        {
            var player = client.Player;
            if (player.tradeAccepted) return;

            player.trade = packet.MyOffers;
            if (player.tradeTarget.trade.SequenceEqual(packet.YourOffers))
            {
                player.tradeAccepted = true;
                player.tradeTarget.Client.SendPacket(new TradeAcceptedPacket()
                {
                    MyOffers = player.tradeTarget.trade,
                    YourOffers = player.trade
                });

                player.MonitorTrade();
                if (player.tradeAccepted && player.tradeTarget.tradeAccepted)
                    client.Manager.Logic.AddPendingAction(t => DoTrade(player));
            }
        }

        void DoTrade(Player player)
        {
            var tradeTarget = player.tradeTarget;
            if (!player.tradeAccepted || !tradeTarget.tradeAccepted)
                return;

            List<Item> thisItems = new List<Item>();
            for (int i = 0; i < player.trade.Length; i++)
                if (player.trade[i])
                {
                    thisItems.Add(player.Inventory[i]);
                    player.Inventory[i] = null;
                }
            List<Item> targetItems = new List<Item>();
            for (int i = 0; i < tradeTarget.trade.Length; i++)
                if (tradeTarget.trade[i])
                {
                    targetItems.Add(tradeTarget.Inventory[i]);
                    tradeTarget.Inventory[i] = null;
                }

            for (int i = 0; i < player.Inventory.Length; i++) //put items by slotType
                if (player.Inventory[i] == null)
                {
                    if (player.SlotTypes[i] == 0)
                    {
                        player.Inventory[i] = targetItems[0];
                        targetItems.RemoveAt(0);
                    }
                    else
                    {
                        int itmIdx = -1;
                        for (int j = 0; j < targetItems.Count; j++)
                            if (targetItems[j].SlotType == player.SlotTypes[i])
                            {
                                itmIdx = j;
                                break;
                            }
                        if (itmIdx != -1)
                        {
                            player.Inventory[i] = targetItems[itmIdx];
                            targetItems.RemoveAt(itmIdx);
                        }
                    }
                    if (targetItems.Count == 0) break;
                }
            if (targetItems.Count > 0)
                for (int i = 0; i < player.Inventory.Length; i++) //force put item
                    if (player.Inventory[i] == null)
                    {
                        player.Inventory[i] = targetItems[0];
                        targetItems.RemoveAt(0);
                        if (targetItems.Count == 0) break;
                    }


            for (int i = 0; i < tradeTarget.Inventory.Length; i++) //put items by slotType
                if (tradeTarget.Inventory[i] == null)
                {
                    if (tradeTarget.SlotTypes[i] == 0)
                    {
                        tradeTarget.Inventory[i] = thisItems[0];
                        thisItems.RemoveAt(0);
                    }
                    else
                    {
                        int itmIdx = -1;
                        for (int j = 0; j < thisItems.Count; j++)
                            if (thisItems[j].SlotType == tradeTarget.SlotTypes[i])
                            {
                                itmIdx = j;
                                break;
                            }
                        if (itmIdx != -1)
                        {
                            tradeTarget.Inventory[i] = thisItems[itmIdx];
                            thisItems.RemoveAt(itmIdx);
                        }
                    }
                    if (thisItems.Count == 0) break;
                }
            if (thisItems.Count > 0)
                for (int i = 0; i < tradeTarget.Inventory.Length; i++) //force put item
                    if (tradeTarget.Inventory[i] == null)
                    {
                        tradeTarget.Inventory[i] = thisItems[0];
                        thisItems.RemoveAt(0);
                        if (thisItems.Count == 0) break;
                    }

            player.UpdateCount++;
            tradeTarget.UpdateCount++;


            player.Client.SendPacket(new TradeDonePacket()
            {
                Result = 0,
                Message = "Trade done!"
            });
            tradeTarget.Client.SendPacket(new TradeDonePacket()
            {
                Result = 0,
                Message = "Trade done!"
            });

            player.ResetTrade();
        }
    }
}
