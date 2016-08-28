using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.svrPackets;
using wServer.networking.cliPackets;
using wServer.logic;
using common;

namespace wServer.realm.entities
{
    public partial class Player
    {
        internal Projectile PlayerShootProjectile(
            byte id, ProjectileDesc desc, ushort objType,
            int time, Position position, float angle)
        {
            projectileId = id;
            return CreateProjectile(desc, objType,
                (int)statsMgr.GetAttackDamage(desc.MinDamage, desc.MaxDamage),
                time + tickMapping, position, angle);
        }

        //public void EnemyHit(RealmTime time, EnemyHitPacket pkt)
        //{
        //    var entity = Owner.GetEntity(pkt.TargetId);
        //    if (entity != null && pkt.Killed)   //Tolerance
        //    {
        //        Projectile prj = (this as IProjectileOwner).Projectiles[pkt.BulletId];
        //        Position? entPos = entity.TryGetHistory((time.tickTimes - tickMapping) - pkt.Time);
        //        Position? prjPos = prj == null ? null : (Position?)prj.GetPosition(pkt.Time + tickMapping - prj.BeginTime);
        //        var tol1 = (entPos == null || prjPos == null) ? 10 : (prjPos.Value.X - entPos.Value.X) * (prjPos.Value.X - entPos.Value.X) + (prjPos.Value.Y - entPos.Value.Y) * (prjPos.Value.Y - entPos.Value.Y);
        //        var tol2 = prj == null ? 10 : (prj.X - entity.X) * (prj.X - entity.X) + (prj.Y - entity.Y) * (prj.Y - entity.Y);
        //        if (prj != null && (tol1 < 1 || tol2 < 1))
        //        {
        //            prj.ForceHit(entity, time);
        //        }
        //        else
        //        {
        //            Console.WriteLine("CAN'T TOLERANT! " + tol1 + " " + tol2);
        //            client.SendPacket(new UpdatePacket()
        //            {
        //                Tiles = new UpdatePacket.TileData[0],
        //                NewObjects = new ObjectDef[] { entity.ToDefinition() },
        //                RemovedObjectIds = new int[] { pkt.TargetId }
        //            });
        //            clientEntities.Remove(entity);
        //        }
        //    }
        //    else if (pkt.Killed)
        //    {
        //        client.SendPacket(new UpdatePacket()
        //        {
        //            Tiles = new UpdatePacket.TileData[0],
        //            NewObjects = Empty<ObjectDef>.Array,
        //            RemovedObjectIds = new int[] { pkt.TargetId }
        //        });
        //    }
        //}
    }
}
