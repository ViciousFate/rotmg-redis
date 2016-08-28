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
    class BuyPacketHandler : PacketHandlerBase<BuyPacket>
    {
        public override PacketID ID { get { return PacketID.Buy; } }

        protected override void HandlePacket(Client client, BuyPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, packet.ObjectId));
        }

        void Handle(Player player, int objId)
        {
            if (player.Owner == null) return;

            SellableObject obj = player.Owner.GetEntity(objId) as SellableObject;
            if (obj != null)
                obj.Buy(player);
        }
    }
}
