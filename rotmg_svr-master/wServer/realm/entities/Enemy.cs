using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.svrPackets;
using wServer.logic;
using wServer.realm.terrain;
using common;

namespace wServer.realm.entities
{
    public class Enemy : Character
    {
        bool stat;

        DamageCounter counter;
        public Enemy(RealmManager manager, ushort objType)
            : base(manager, objType, new wRandom())
        {
            stat = ObjectDesc.MaxHP == 0;
            counter = new DamageCounter(this);
        }

        public DamageCounter DamageCounter { get { return counter; } }

        public WmapTerrain Terrain { get; set; }

        public int AltTextureIndex { get; set; }
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.AltTextureIndex] = AltTextureIndex;
            base.ExportStats(stats);
        }
        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.AltTextureIndex)
                AltTextureIndex = (int)val;
            base.ImportStats(stats, val);
        }

        Position? pos;
        public Position SpawnPoint { get { return pos ?? new Position() { X = X, Y = Y }; } }

        public override void Init(World owner)
        {
            base.Init(owner);
            if (ObjectDesc.StasisImmune)
                ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.StasisImmune,
                    DurationMS = -1
                });
        }

        public void Death(RealmTime time)
        {
            counter.Death(time);
            if (CurrentState != null)
                CurrentState.OnDeath(new BehaviorEventArgs(this, time));
            Owner.LeaveWorld(this);
        }

        public int Damage(Player from, RealmTime time, int dmg, bool noDef, params ConditionEffect[] effs)
        {
            if (stat) return 0;
            if (HasConditionEffect(ConditionEffects.Invincible))
                return 0;
            if (!HasConditionEffect(ConditionEffects.Paused) &&
                !HasConditionEffect(ConditionEffects.Stasis))
            {
                var def = this.ObjectDesc.Defense;
                if (noDef)
                    def = 0;
                dmg = (int)StatsManager.GetDefenseDamage(this, dmg, def);
                int effDmg = dmg;
                if (effDmg > HP)
                    effDmg = HP;
                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= dmg;
                ApplyConditionEffect(effs);
                Owner.BroadcastPacket(new DamagePacket()
                {
                    TargetId = this.Id,
                    Effects = 0,
                    Damage = (ushort)dmg,
                    Killed = HP < 0,
                    BulletId = 0,
                    ObjectId = from.Id
                }, null);

                counter.HitBy(from, time, null, dmg);

                if (HP < 0 && Owner != null)
                {
                    Death(time);
                }

                UpdateCount++;
                return effDmg;
            }
            return 0;
        }
        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (stat) return false;
            if (HasConditionEffect(ConditionEffects.Invincible))
                return false;
            if (projectile.ProjectileOwner is Player &&
                !HasConditionEffect(ConditionEffects.Paused) &&
                !HasConditionEffect(ConditionEffects.Stasis))
            {
                var def = this.ObjectDesc.Defense;
                if (projectile.Descriptor.ArmorPiercing)
                    def = 0;
                int dmg = (int)StatsManager.GetDefenseDamage(this, projectile.Damage, def);
                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= dmg;
                ApplyConditionEffect(projectile.Descriptor.Effects);
                Owner.BroadcastPacket(new DamagePacket()
                {
                    TargetId = this.Id,
                    Effects = projectile.ConditionEffects,
                    Damage = (ushort)dmg,
                    Killed = HP < 0,
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.ProjectileOwner.Self.Id
                }, projectile.ProjectileOwner as Player);

                counter.HitBy(projectile.ProjectileOwner as Player, time, projectile, dmg);

                if (HP < 0 && Owner != null)
                {
                    Death(time);
                }
                UpdateCount++;
                return true;
            }
            return false;
        }

        float bleeding = 0;
        public override void Tick(RealmTime time)
        {
            if (pos == null)
                pos = new Position() { X = X, Y = Y };

            if (!stat && HasConditionEffect(ConditionEffects.Bleeding))
            {
                if (bleeding > 1)
                {
                    HP -= (int)bleeding;
                    bleeding -= (int)bleeding;
                    UpdateCount++;
                }
                bleeding += 28 * (time.thisTickTimes / 1000f);
            }
            base.Tick(time);
        }
    }
}
