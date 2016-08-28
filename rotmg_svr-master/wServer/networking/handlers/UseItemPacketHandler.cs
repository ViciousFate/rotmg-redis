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
    class UseItemPacketHandler : PacketHandlerBase<UseItemPacket>
    {
        public override PacketID ID { get { return PacketID.UseItem; } }

        protected override void HandlePacket(Client client, UseItemPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet));
        }

        void Handle(Player player, RealmTime time, UseItemPacket packet)
        {
            if (player.Owner == null) return;

            player.UseItem(time, packet.Slot.ObjectId, packet.Slot.SlotId, packet.Position);
        }
    }
}
