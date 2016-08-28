using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using wServer.realm.worlds;
using wServer.logic;
using wServer.networking.svrPackets;
using wServer.networking.cliPackets;
using wServer.networking;
using wServer.realm.terrain;
using log4net;
using common;

namespace wServer.realm.entities
{
    interface IPlayer
    {
        void Damage(int dmg, Character chr);
        bool IsVisibleToEnemy();
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        static ILog log = LogManager.GetLogger(typeof(Player));

        Client client;
        public Client Client { get { return client; } }

        //Stats
        public int AccountId { get; private set; }

        public int Experience { get; set; }
        public int ExperienceGoal { get; set; }
        public int Level { get; set; }

        public int CurrentFame { get; set; }
        public int Fame { get; set; }
        public int FameGoal { get; set; }
        public int Stars { get; set; }

        public string Guild { get; set; }
        public int GuildRank { get; set; }

        public int Credits { get; set; }
        public bool NameChosen { get; set; }
        public int Texture1 { get; set; }
        public int Texture2 { get; set; }

        public bool Glowing { get; set; }
        public int MP { get; set; }

        public int[] SlotTypes { get; private set; }
        public Inventory Inventory { get; private set; }
        public int[] Stats { get; private set; }
        public int[] Boost { get; private set; }

        protected override void ImportStats(StatsType stats, object val)
        {
            base.ImportStats(stats, val);
            switch (stats)
            {
                case StatsType.AccountId: AccountId = (int)val; break;

                case StatsType.Experience: Experience = (int)val; break;
                case StatsType.ExperienceGoal: ExperienceGoal = (int)val; break;
                case StatsType.Level: Level = (int)val; break;

                case StatsType.Fame: CurrentFame = (int)val; break;
                case StatsType.CurrentFame: Fame = (int)val; break;
                case StatsType.FameGoal: FameGoal = (int)val; break;
                case StatsType.Stars: Stars = (int)val; break;

                case StatsType.Guild: Guild = (string)val; break;
                case StatsType.GuildRank: GuildRank = (int)val; break;

                case StatsType.Credits: Credits = (int)val; break;
                case StatsType.NameChosen: NameChosen = (int)val != 0 ? true : false; break;
                case StatsType.Texture1: Texture1 = (int)val; break;
                case StatsType.Texture2: Texture2 = (int)val; break;

                case StatsType.Glowing: Glowing = (int)val != 0 ? true : false; break;
                case StatsType.HP: HP = (int)val; break;
                case StatsType.MP: MP = (int)val; break;

                case StatsType.Inventory0: Inventory[0] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory1: Inventory[1] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory2: Inventory[2] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory3: Inventory[3] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory4: Inventory[4] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory5: Inventory[5] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory6: Inventory[6] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory7: Inventory[7] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory8: Inventory[8] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory9: Inventory[9] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory10: Inventory[10] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory11: Inventory[11] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;

                case StatsType.MaximumHP: Stats[0] = (int)val; break;
                case StatsType.MaximumMP: Stats[1] = (int)val; break;
                case StatsType.Attack: Stats[2] = (int)val; break;
                case StatsType.Defense: Stats[3] = (int)val; break;
                case StatsType.Speed: Stats[4] = (int)val; break;
                case StatsType.Vitality: Stats[5] = (int)val; break;
                case StatsType.Wisdom: Stats[6] = (int)val; break;
                case StatsType.Dexterity: Stats[7] = (int)val; break;
            }
        }
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            base.ExportStats(stats);
            stats[StatsType.AccountId] = AccountId;

            stats[StatsType.Experience] = Experience - GetLevelExp(Level);
            stats[StatsType.ExperienceGoal] = ExperienceGoal;
            stats[StatsType.Level] = Level;

            stats[StatsType.CurrentFame] = CurrentFame;
            stats[StatsType.Fame] = Fame;
            stats[StatsType.FameGoal] = FameGoal;
            stats[StatsType.Stars] = Stars;

            stats[StatsType.Guild] = Guild;
            stats[StatsType.GuildRank] = GuildRank;

            stats[StatsType.Credits] = Credits;
            stats[StatsType.NameChosen] = NameChosen ? 1 : 0;
            stats[StatsType.Texture1] = Texture1;
            stats[StatsType.Texture2] = Texture2;

            stats[StatsType.Glowing] = Glowing ? 1 : 0;
            stats[StatsType.HP] = HP;
            stats[StatsType.MP] = MP;

            stats[StatsType.Inventory0] = (Inventory[0] != null ? Inventory[0].ObjectType : -1);
            stats[StatsType.Inventory1] = (Inventory[1] != null ? Inventory[1].ObjectType : -1);
            stats[StatsType.Inventory2] = (Inventory[2] != null ? Inventory[2].ObjectType : -1);
            stats[StatsType.Inventory3] = (Inventory[3] != null ? Inventory[3].ObjectType : -1);
            stats[StatsType.Inventory4] = (Inventory[4] != null ? Inventory[4].ObjectType : -1);
            stats[StatsType.Inventory5] = (Inventory[5] != null ? Inventory[5].ObjectType : -1);
            stats[StatsType.Inventory6] = (Inventory[6] != null ? Inventory[6].ObjectType : -1);
            stats[StatsType.Inventory7] = (Inventory[7] != null ? Inventory[7].ObjectType : -1);
            stats[StatsType.Inventory8] = (Inventory[8] != null ? Inventory[8].ObjectType : -1);
            stats[StatsType.Inventory9] = (Inventory[9] != null ? Inventory[9].ObjectType : -1);
            stats[StatsType.Inventory10] = (Inventory[10] != null ? Inventory[10].ObjectType : -1);
            stats[StatsType.Inventory11] = (Inventory[11] != null ? Inventory[11].ObjectType : -1);

            if (Boost == null) CalculateBoost();

            stats[StatsType.MaximumHP] = Stats[0] + Boost[0];
            stats[StatsType.MaximumMP] = Stats[1] + Boost[1];
            stats[StatsType.Attack] = Stats[2] + Boost[2];
            stats[StatsType.Defense] = Stats[3] + Boost[3];
            stats[StatsType.Speed] = Stats[4] + Boost[4];
            stats[StatsType.Vitality] = Stats[5] + Boost[5];
            stats[StatsType.Wisdom] = Stats[6] + Boost[6];
            stats[StatsType.Dexterity] = Stats[7] + Boost[7];

            stats[StatsType.HPBoost] = Boost[0];
            stats[StatsType.MPBoost] = Boost[1];
            stats[StatsType.AttackBonus] = Boost[2];
            stats[StatsType.DefenseBonus] = Boost[3];
            stats[StatsType.SpeedBonus] = Boost[4];
            stats[StatsType.VitalityBonus] = Boost[5];
            stats[StatsType.WisdomBonus] = Boost[6];
            stats[StatsType.DexterityBonus] = Boost[7];
        }
        public void SaveToCharacter()
        {
            var chr = client.Character;
            chr.Level = Level;
            chr.Experience = Experience;
            chr.Fame = Fame;
            chr.Items = Inventory.Select(_ => _ == null ? (ushort)0xffff : _.ObjectType).ToArray();
            chr.HP = HP;
            chr.MP = MP;
            chr.Stats = Stats;
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.FameStats = FameCounter.Stats.Write();
            chr.LastSeen = DateTime.Now;
        }

        void CalculateBoost()
        {
            if (Boost == null) Boost = new int[12];
            else
                for (int i = 0; i < Boost.Length; i++) Boost[i] = 0;
            for (int i = 0; i < 4; i++)
            {
                if (Inventory[i] == null) continue;
                foreach (var b in Inventory[i].StatsBoost)
                {
                    switch ((StatsType)b.Key)
                    {
                        case StatsType.MaximumHP: Boost[0] += b.Value; break;
                        case StatsType.MaximumMP: Boost[1] += b.Value; break;
                        case StatsType.Attack: Boost[2] += b.Value; break;
                        case StatsType.Defense: Boost[3] += b.Value; break;
                        case StatsType.Speed: Boost[4] += b.Value; break;
                        case StatsType.Vitality: Boost[5] += b.Value; break;
                        case StatsType.Wisdom: Boost[6] += b.Value; break;
                        case StatsType.Dexterity: Boost[7] += b.Value; break;
                    }
                }
            }
        }


        StatsManager statsMgr;
        public Player(Client client)
            : base(client.Manager, (ushort)client.Character.ObjectType, client.Random)
        {
            this.client = client;
            statsMgr = new StatsManager(this);
            Name = client.Account.Name;
            AccountId = client.Account.AccountId;

            Level = client.Character.Level;
            Experience = client.Character.Experience;
            Texture1 = client.Character.Tex1;
            Texture2 = client.Character.Tex2;
            Credits = client.Account.Credits;
            NameChosen = client.Account.NameChosen;
            CurrentFame = client.Account.Fame;
            Fame = client.Character.Fame;
            Glowing = true;
            Guild = "";
            GuildRank = -1;
            HP = client.Character.HP;
            MP = client.Character.MP;
            ConditionEffects = 0;

            Inventory = new Inventory(this,
                client.Character.Items
                    .Select(_ => _ == 0xffff ? null : client.Manager.GameData.Items[_])
                    .ToArray());
            Inventory.InventoryChanged += (sender, e) => CalculateBoost();
            SlotTypes = client.Manager.GameData.ObjectTypeToElement[ObjectType]
                .Element("SlotTypes").Value.CommaToArray<int>();
            Stats = (int[])client.Character.Stats.Clone();
        }

        byte[,] tiles;
        public FameCounter FameCounter { get; private set; }

        public override void Init(World owner)
        {
            Random rand = new System.Random();
            int x, y;
            do
            {
                x = rand.Next(0, owner.Map.Width);
                y = rand.Next(0, owner.Map.Height);
            } while (owner.Map[x, y].Region != TileRegion.Spawn);
            Move(x + 0.5f, y + 0.5f);
            tiles = new byte[owner.Map.Width, owner.Map.Height];

            FameCounter = new FameCounter(this);
            FameGoal = GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
            ExperienceGoal = GetExpGoal(client.Character.Level);
            Stars = GetStars();

            SetNewbiePeriod();
            base.Init(owner);
        }

        public override void Tick(RealmTime time)
        {
            if (client.Stage == ProtocalStage.Disconnected)
            {
                Owner.LeaveWorld(this);
                return;
            }
            if (!KeepAlive(time)) return;

            if (Boost == null) CalculateBoost();

            CheckTradeTimeout(time);
            HandleRegen(time);
            HandleQuest(time);
            HandleGround(time);
            HandleEffects(time);
            FameCounter.Tick(time);

            SendUpdate(time);

            if (HP <= 0)
            {
                Death("Unknown");
                return;
            }

            base.Tick(time);
        }

        float hpRegenCounter;
        float mpRegenCounter;
        void HandleRegen(RealmTime time)
        {
            if (HP == Stats[0] + Boost[0] || !CanHpRegen())
                hpRegenCounter = 0;
            else
            {
                hpRegenCounter += statsMgr.GetHPRegen() * time.thisTickTimes / 1000f;
                int regen = (int)hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + regen);
                    hpRegenCounter -= regen;
                    UpdateCount++;
                }
            }

            if (MP == Stats[1] + Boost[1] || !CanMpRegen())
                mpRegenCounter = 0;
            else
            {
                mpRegenCounter += statsMgr.GetMPRegen() * time.thisTickTimes / 1000f;
                int regen = (int)mpRegenCounter;
                if (regen > 0)
                {
                    MP = Math.Min(Stats[1] + Boost[1], MP + regen);
                    mpRegenCounter -= regen;
                    UpdateCount++;
                }
            }
        }

        public void Teleport(RealmTime time, int objId)
        {
            if (!this.TPCooledDown())
            {
                SendError("Too soon to teleport again!");
                return;
            }
            SetTPDisabledPeriod();
            var obj = Owner.GetEntity(objId);
            if (obj == null) return;
            Move(obj.X, obj.Y);
            FameCounter.Teleport();
            SetNewbiePeriod();
            UpdateCount++;
            Owner.BroadcastPacket(new GotoPacket()
            {
                ObjectId = Id,
                Position = new Position()
                {
                    X = X,
                    Y = Y
                }
            }, null);
            Owner.BroadcastPacket(new ShowEffectPacket()
            {
                EffectType = EffectType.Teleport,
                TargetId = Id,
                PosA = new Position()
                {
                    X = X,
                    Y = Y
                },
                Color = new ARGB(0xFFFFFFFF)
            }, null);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player ||
                HasConditionEffect(ConditionEffects.Paused) ||
                HasConditionEffect(ConditionEffects.Stasis) ||
                HasConditionEffect(ConditionEffects.Invincible))
                return false;

            var dmg = (int)statsMgr.GetDefenseDamage(projectile.Damage, projectile.Descriptor.ArmorPiercing);
            if (!HasConditionEffect(ConditionEffects.Invulnerable))
                HP -= dmg;
            ApplyConditionEffect(projectile.Descriptor.Effects);
            UpdateCount++;
            Owner.BroadcastPacket(new DamagePacket()
            {
                TargetId = this.Id,
                Effects = HasConditionEffect(ConditionEffects.Invincible) ? 0 : projectile.ConditionEffects,
                Damage = (ushort)dmg,
                Killed = HP <= 0,
                BulletId = projectile.ProjectileId,
                ObjectId = projectile.ProjectileOwner.Self.Id
            }, this);

            if (HP <= 0) Death(
                projectile.ProjectileOwner.Self.ObjectDesc.DisplayId ??
                projectile.ProjectileOwner.Self.ObjectDesc.ObjectId);

            return base.HitByProjectile(projectile, time);
        }

        public void Damage(int dmg, Character chr)
        {
            if (HasConditionEffect(ConditionEffects.Paused) ||
                HasConditionEffect(ConditionEffects.Stasis) ||
                HasConditionEffect(ConditionEffects.Invincible))
                return;

            dmg = (int)statsMgr.GetDefenseDamage(dmg, false);
            if (!HasConditionEffect(ConditionEffects.Invulnerable))
                HP -= dmg;
            UpdateCount++;
            Owner.BroadcastPacket(new DamagePacket()
            {
                TargetId = this.Id,
                Effects = 0,
                Damage = (ushort)dmg,
                Killed = HP <= 0,
                BulletId = 0,
                ObjectId = chr.Id
            }, this);

            if (HP <= 0) Death(
                chr.ObjectDesc.DisplayId ??
                chr.ObjectDesc.ObjectId);
        }

        bool resurrecting = false;
        bool CheckResurrection()
        {
            for (int i = 0; i < 4; i++)
            {
                Item item = Inventory[i];
                if (item == null || !item.Resurrects) continue;

                HP = Stats[0] + Stats[0];
                MP = Stats[1] + Stats[1];
                Inventory[i] = null;
                foreach (var player in Owner.Players.Values)
                    player.SendInfo(string.Format("{0}'s {1} breaks and he disappears", Name, item.ObjectId));

                client.Reconnect(new ReconnectPacket()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array,
                });

                resurrecting = true;
                return true;
            }
            return false;
        }
        void GenerateGravestone()
        {
            int maxed = 0;
            foreach (var i in Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease"))
            {
                int limit = int.Parse(Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value).Attribute("max").Value);
                int idx = StatsManager.StatsNameToIndex(i.Value);
                if (Stats[idx] >= limit)
                    maxed++;
            }

            ushort objType;
            int? time;
            switch (maxed)
            {
                case 8:
                    objType = 0x0735; time = null;
                    break;
                case 7:
                    objType = 0x0734; time = null;
                    break;
                case 6:
                    objType = 0x072b; time = null;
                    break;
                case 5:
                    objType = 0x072a; time = null;
                    break;
                case 4:
                    objType = 0x0729; time = null;
                    break;
                case 3:
                    objType = 0x0728; time = null;
                    break;
                case 2:
                    objType = 0x0727; time = null;
                    break;
                case 1:
                    objType = 0x0726; time = null;
                    break;
                default:
                    if (Level <= 1)
                    {
                        objType = 0x0723; time = 30 * 1000;
                    }
                    else if (Level < 20)
                    {
                        objType = 0x0724; time = 60 * 1000;
                    }
                    else
                    {
                        objType = 0x0725; time = 5 * 60 * 1000;
                    }
                    break;
            }
            StaticObject obj = new StaticObject(Manager, objType, time, true, time == null ? false : true, false);
            obj.Move(X, Y);
            obj.Name = this.Name;
            Owner.EnterWorld(obj);
        }

        public void Death(string killer)
        {
            if (client.Stage == ProtocalStage.Disconnected || resurrecting)
                return;
            if (CheckResurrection())
                return;

            GenerateGravestone();
            foreach (var i in Owner.Players.Values)
                i.SendInfo(Name + " died at Level " + Level + ", with " + Fame + " Fame" +/* " and " + Experience + " Experience " + */", killed by " + killer); //removed XP as max packet length reached!

            SaveToCharacter();
            Manager.Database.SaveCharacter(client.Account, client.Character, true);
            Manager.Database.Death(Manager.GameData, client.Account,
                client.Character, FameCounter.Stats, killer);
            client.SendPacket(new DeathPacket()
            {
                AccountId = AccountId,
                CharId = client.Character.CharId,
                Killer = killer
            });
            Owner.Timers.Add(new WorldTimer(1000, (w, t) => client.Disconnect()));
            Owner.LeaveWorld(this);
        }
    }
}