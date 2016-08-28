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
    class TeleportPacketHandler : PacketHandlerBase<TeleportPacket>
    {
        public override PacketID ID { get { return PacketID.Teleport; } }

        protected override void HandlePacket(Client client, TeleportPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet.ObjectId));
        }

        void Handle(Player player, RealmTime time, int objId)
        {
            if (player.Owner == null) return;

            player.Teleport(time, objId);
        }
    }
}
