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
    class EnemyHitPacketHandler : PacketHandlerBase<EnemyHitPacket>
    {
        public override PacketID ID { get { return PacketID.EnemyHit; } }

        protected override void HandlePacket(Client client, EnemyHitPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
                Handle(client.Player, t, packet.TargetId, packet.BulletId));
        }

        void Handle(Player player, RealmTime time, int targetId, byte bulletId)
        {
            if (player.Owner == null) return;
            var entity = player.Owner.GetEntity(targetId);
            if (entity != null)   //Tolerance
            {
                var prj = (player as IProjectileOwner).Projectiles[bulletId];
                if (prj != null)
                    prj.ForceHit(entity, time);
            }
        }
    }
}
