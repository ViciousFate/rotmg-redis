using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.realm;
using common;

namespace wServer.logic
{
    public class FameCounter
    {
        Player player;
        public Player Host { get { return player; } }

        public FameStats Stats { get; private set; }
        public DbClassStats ClassStats { get; private set; }
        public FameCounter(Player player)
        {
            this.player = player;
            this.Stats = FameStats.Read(player.Client.Character.FameStats);
            this.ClassStats = new DbClassStats(player.Client.Account);
        }

        HashSet<Projectile> projs = new HashSet<Projectile>();
        public void Shoot(Projectile proj)
        {
            Stats.Shots++;
            projs.Add(proj);
        }

        public void Hit(Projectile proj, Enemy enemy)
        {
            if (projs.Contains(proj))
            {
                projs.Remove(proj);
                Stats.ShotsThatDamage++;
            }
        }

        public void Killed(Enemy enemy, bool killer)
        {
            if (enemy.ObjectDesc.God)
                Stats.GodAssists++;
            else
                Stats.MonsterAssists++;
            if (player.Quest == enemy)
                Stats.QuestsCompleted++;
            if (killer)
            {
                if (enemy.ObjectDesc.God)
                    Stats.GodKills++;
                else
                    Stats.MonsterKills++;

                if (enemy.ObjectDesc.Cube)
                    Stats.CubeKills++;
                if (enemy.ObjectDesc.Oryx)
                    Stats.OryxKills++;
            }
        }
        public void LevelUpAssist(int count)
        {
            Stats.LevelUpAssists += count;
        }

        public void TileSent(int num)
        {
            Stats.TilesUncovered += num;
        }

        public void Teleport()
        {
            Stats.Teleports++;
        }

        public void UseAbility()
        {
            Stats.SpecialAbilityUses++;
        }

        public void DrinkPot()
        {
            Stats.PotionsDrunk++;
        }

        int elapsed = 0;
        public void Tick(RealmTime time)
        {
            elapsed += time.thisTickTimes;
            if (elapsed > 1000 * 60)
            {
                elapsed -= 1000 * 60;
                Stats.MinutesActive++;
            }
        }
    }
}
