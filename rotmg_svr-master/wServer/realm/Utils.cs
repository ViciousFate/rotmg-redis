using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using common;
using wServer.realm.entities;

namespace wServer
{
    static class EntityUtils
    {
        public static double DistSqr(this Entity a, Entity b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }
        public static double Dist(this Entity a, Entity b)
        {
            return Math.Sqrt(a.DistSqr(b));
        }


        public static bool AnyPlayerNearby(this Entity entity)
        {
            foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, 16))
            {
                var d = i.Dist(entity);
                if (d < 16 * 16)
                    return true;
            }
            return false;
        }
        public static bool AnyPlayerNearby(this World world, double x, double y)
        {
            foreach (var i in world.PlayersCollision.HitTest(x, y, 16))
            {
                var d = MathsUtils.Dist(i.X, i.Y, x, y);
                if (d < 16 * 16)
                    return true;
            }
            return false;
        }

        public static Entity GetNearestEntity(this Entity entity, double dist, ushort? objType)   //Null for player
        {
            return entity.GetNearestEntities(dist, objType).FirstOrDefault();
        }
        public static IEnumerable<Entity> GetNearestEntities(this Entity entity, double dist, ushort? objType)   //Null for player
        {
            if (entity.Owner == null) yield break;
            if (objType == null)
                foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (!(i as IPlayer).IsVisibleToEnemy()) continue;
                    var d = i.Dist(entity);
                    if (d < dist)
                        yield return i;
                }
            else
                foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (i.ObjectType != objType.Value) continue;
                    var d = i.Dist(entity);
                    if (d < dist)
                        yield return i;
                }
        }
        public static Entity GetNearestEntity(this Entity entity, double dist, bool players, Predicate<Entity> predicate = null)
        {
            if (entity.Owner == null) return null;
            Entity ret = null;
            if (players)
                foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (!(i as IPlayer).IsVisibleToEnemy() ||
                        i == entity) continue;
                    var d = i.Dist(entity);
                    if (d < dist)
                    {
                        if (predicate != null && !predicate(i))
                            continue;
                        dist = d;
                        ret = i;
                    }
                }
            else
                foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (i == entity) continue;
                    var d = i.Dist(entity);
                    if (d < dist)
                    {
                        if (predicate != null && !predicate(i))
                            continue;
                        dist = d;
                        ret = i;
                    }
                }
            return ret;
        }

        public static Entity GetNearestEntityByGroup(this Entity entity, double dist, string group)
        {
            return entity.GetNearestEntitiesByGroup(dist, group).FirstOrDefault();
        }
        public static IEnumerable<Entity> GetNearestEntitiesByGroup(this Entity entity, double dist, string group)
        {
            if (entity.Owner == null)
                yield break;
            foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, dist))
            {
                if (i.ObjectDesc == null || i.ObjectDesc.Group != group) continue;
                var d = i.Dist(entity);
                if (d < dist)
                    yield return i;
            }
        }

        public static int CountEntity(this Entity entity, double dist, ushort? objType)
        {
            if (entity.Owner == null) return 0;
            int ret = 0;
            if (objType == null)
                foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (!(i as IPlayer).IsVisibleToEnemy()) continue;
                    var d = i.Dist(entity);
                    if (d < dist)
                        ret++;
                }
            else
                foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (i.ObjectType != objType.Value) continue;
                    var d = i.Dist(entity);
                    if (d < dist)
                        ret++;
                }
            return ret;
        }
        public static int CountEntity(this Entity entity, double dist, string group)
        {
            if (entity.Owner == null) return 0;
            int ret = 0;
            foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, dist))
            {
                if (i.ObjectDesc == null || i.ObjectDesc.Group != group) continue;
                var d = i.Dist(entity);
                if (d < dist)
                    ret++;
            }
            return ret;
        }

        public static float GetSpeed(this Entity entity, float spd)
        {
            return 5.55f * spd + 0.74f;
        }

        public static bool ValidateAndMove(this Entity entity, float x, float y)
        {
            if (entity.Owner == null ||
                entity.HasConditionEffect(ConditionEffects.Paralyzed)) return false;
            if (entity.Validate(x, y))
                entity.Move(x, y);
            else if (entity.Validate(entity.X, y))
                entity.Move(entity.X, y);
            else if (entity.Validate(x, entity.Y))
                entity.Move(x, entity.Y);
            else
                return false;
            return true;
        }
        public static bool Validate(this Entity entity, float x, float y)
        {
            if (entity.Owner == null ||
                entity.HasConditionEffect(ConditionEffects.Paralyzed)) return false;
            if (x < 0 || x >= entity.Owner.Map.Width ||
                y < 0 || y >= entity.Owner.Map.Height)
                return false;
            if (!entity.Owner.IsPassable((int)x, (int)y)) return false;

            return true;
        }

        public static void AOE(this Entity entity, float radius, ushort? objType, Action<Entity> callback)   //Null for player
        {
            if (objType == null)
                foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, radius))
                {
                    var d = i.Dist(entity);
                    if (d < radius)
                        callback(i);
                }
            else
                foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, radius))
                {
                    if (i.ObjectType != objType.Value) continue;
                    var d = i.Dist(entity);
                    if (d < radius)
                        callback(i);
                }
        }
        public static void AOE(this Entity entity, float radius, bool players, Action<Entity> callback)   //Null for player
        {
            if (players)
                foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, radius))
                {
                    var d = i.Dist(entity);
                    if (d < radius)
                        callback(i);
                }
            else
                foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, radius))
                {
                    if (!(i is Enemy)) continue;
                    var d = i.Dist(entity);
                    if (d < radius)
                        callback(i);
                }
        }
        public static void AOE(this World world, Position pos, float radius, bool players, Action<Entity> callback)   //Null for player
        {
            if (players)
                foreach (var i in world.PlayersCollision.HitTest(pos.X, pos.Y, radius))
                {
                    var d = MathsUtils.Dist(i.X, i.Y, pos.X, pos.Y);
                    if (d < radius)
                        callback(i);
                }
            else
                foreach (var i in world.EnemiesCollision.HitTest(pos.X, pos.Y, radius))
                {
                    if (!(i is Enemy)) continue;
                    var d = MathsUtils.Dist(i.X, i.Y, pos.X, pos.Y);
                    if (d < radius)
                        callback(i);
                }
        }
    }

    static class ItemUtils
    {
        public static bool AuditItem(this IContainer container, Item item, int slot)
        {
            return item == null || container.SlotTypes[slot] == 0 || item.SlotType == container.SlotTypes[slot];
        }
    }
}
