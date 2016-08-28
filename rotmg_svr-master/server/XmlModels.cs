using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using common;

namespace server
{
    class ServerItem
    {
        public string Name { get; set; }
        public string DNS { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double Usage { get; set; }
        public bool AdminOnly { get; set; }

        public XElement ToXml()
        {
            return
                new XElement("Server",
                    new XElement("Name", Name),
                    new XElement("DNS", DNS),
                    new XElement("Lat", Lat),
                    new XElement("Long", Long),
                    new XElement("Usage", Usage),
                    new XElement("AdminOnly", AdminOnly)
                );
        }
    }
    class NewsItem
    {
        public string Icon { get; internal set; }
        public string Title { get; internal set; }
        public string TagLine { get; internal set; }
        public string Link { get; internal set; }
        public DateTime Date { get; internal set; }

        public static NewsItem FromDb(DbNewsEntry entry)
        {
            return new NewsItem()
            {
                Icon = entry.Icon,
                Title = entry.Title,
                TagLine = entry.Text,
                Link = entry.Link,
                Date = entry.Date
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Item",
                    new XElement("Icon", Icon),
                    new XElement("Title", Title),
                    new XElement("TagLine", TagLine),
                    new XElement("Link", Link),
                    new XElement("Date", Date.ToUnixTimestamp())
                );
        }
    }

    class Guild
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Rank { get; private set; }

        public XElement ToXml()
        {
            return
                new XElement("Guild",
                    new XAttribute("id", Id),
                    new XElement("Name", Name),
                    new XElement("Rank", Rank)
                );
        }
    }

    class ClassStatsEntry
    {
        public ushort ObjectType { get; private set; }
        public int BestLevel { get; private set; }
        public int BestFame { get; private set; }

        public static ClassStatsEntry FromDb(ushort objType, DbClassStatsEntry entry)
        {
            return new ClassStatsEntry()
            {
                ObjectType = objType,
                BestLevel = entry.BestLevel,
                BestFame = entry.BestFame
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("ClassStats",
                    new XAttribute("objectType", ObjectType.To4Hex()),
                    new XElement("BestLevel", BestLevel),
                    new XElement("BestFame", BestFame)
                );
        }
    }
    class Stats
    {
        public int BestCharFame { get; private set; }
        public int TotalFame { get; private set; }
        public int Fame { get; private set; }

        Dictionary<ushort, ClassStatsEntry> entries;
        public ClassStatsEntry this[ushort objType]
        {
            get { return entries[objType]; }
        }

        public static Stats FromDb(DbAccount acc, DbClassStats stats)
        {
            Stats ret = new Stats()
            {
                TotalFame = acc.TotalFame,
                Fame = acc.Fame,
                entries = new Dictionary<ushort, ClassStatsEntry>(),
                BestCharFame = 0
            };
            foreach (var i in stats.AllKeys)
            {
                var objType = ushort.Parse(i);
                var entry = ClassStatsEntry.FromDb(objType, stats[objType]);
                if (entry.BestFame > ret.BestCharFame) ret.BestCharFame = entry.BestFame;
                ret.entries[objType] = entry;
            }
            return ret;
        }

        public XElement ToXml()
        {
            return
                new XElement("Stats",
                    entries.Values.Select(x => x.ToXml()),
                    new XElement("BestCharFame", BestCharFame),
                    new XElement("TotalFame", TotalFame),
                    new XElement("Fame", Fame)
                );
        }
    }

    class Vault
    {
        ushort[][] chests;
        public ushort[] this[int index]
        {
            get { return chests[index]; }
        }

        public static Vault FromDb(DbAccount acc, DbVault vault)
        {
            return new Vault()
            {
                chests = Enumerable.Range(1, acc.VaultCount).
                            Select(x => vault[x] ??
                                Enumerable.Repeat((ushort)0xffff, 8).ToArray()).ToArray()
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Vault",
                    chests.Select(x => new XElement("Chest", x.ToCommaSepString()))
                );
        }
    }

    class Account
    {
        public int AccountId { get; private set; }
        public string Name { get; set; }

        public bool NameChosen { get; private set; }
        public bool Converted { get; private set; }
        public bool Admin { get; private set; }
        public bool VerifiedEmail { get; private set; }

        public int Credits { get; private set; }
        public int NextCharSlotPrice { get; private set; }
        public int BeginnerPackageTimeLeft { get; private set; }

        public Vault Vault { get; private set; }
        public Stats Stats { get; private set; }
        public Guild Guild { get; private set; }

        public static Account FromDb(DbAccount acc)
        {
            return new Account()
            {
                AccountId = acc.AccountId,
                Name = acc.Name,

                NameChosen = acc.NameChosen,
                Converted = false,
                Admin = acc.Admin,
                VerifiedEmail = acc.Verified,

                Credits = acc.Credits,
                NextCharSlotPrice = 100,
                BeginnerPackageTimeLeft = 0,

                Vault = Vault.FromDb(acc, new DbVault(acc)),
                Stats = Stats.FromDb(acc, new DbClassStats(acc)),
                Guild = new Guild()
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Account",
                    new XElement("AccountId", AccountId),
                    new XElement("Name", Name),
                    NameChosen ? new XElement("NameChosen", "") : null,
                    Converted ? new XElement("Converted", "") : null,
                    Admin ? new XElement("Admin", "") : null,
                    VerifiedEmail ? new XElement("VerifiedEmail", "") : null,
                    new XElement("Credits", Credits),
                    new XElement("NextCharSlotPrice", NextCharSlotPrice),
                    new XElement("BeginnerPackageTimeLeft", BeginnerPackageTimeLeft),
                    Vault.ToXml(),
                    Stats.ToXml(),
                    Guild.ToXml()
                );
        }
    }

    class Character
    {
        public int CharacterId { get; private set; }
        public ushort ObjectType { get; private set; }
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int CurrentFame { get; private set; }
        public ushort[] Equipment { get; private set; }
        public int MaxHitPoints { get; private set; }
        public int HitPoints { get; private set; }
        public int MaxMagicPoints { get; private set; }
        public int MagicPoints { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Speed { get; private set; }
        public int Dexterity { get; private set; }
        public int HpRegen { get; private set; }
        public int MpRegen { get; private set; }
        public int Tex1 { get; private set; }
        public int Tex2 { get; private set; }
        public FameStats PCStats { get; private set; }
        public bool Dead { get; private set; }
        public int Pet { get; private set; }

        public static Character FromDb(DbChar character, bool dead)
        {
            return new Character()
            {
                CharacterId = character.CharId,
                ObjectType = character.ObjectType,
                Level = character.Level,
                Exp = character.Experience,
                CurrentFame = character.Fame,
                Equipment = character.Items,
                MaxHitPoints = character.Stats[0],
                MaxMagicPoints = character.Stats[1],
                Attack = character.Stats[2],
                Defense = character.Stats[3],
                Speed = character.Stats[4],
                Dexterity = character.Stats[5],
                HpRegen = character.Stats[6],
                MpRegen = character.Stats[7],
                HitPoints = character.HP,
                MagicPoints = character.MP,
                Tex1 = character.Tex1,
                Tex2 = character.Tex2,
                PCStats = FameStats.Read(character.FameStats),
                Dead = dead,
                Pet = character.Pet
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Char",
                    new XAttribute("id", CharacterId),
                    new XElement("ObjectType", ObjectType),
                    new XElement("Level", Level),
                    new XElement("Exp", Exp),
                    new XElement("CurrentFame", CurrentFame),
                    new XElement("Equipment", Equipment.ToCommaSepString()),
                    new XElement("MaxHitPoints", MaxHitPoints),
                    new XElement("HitPoints", HitPoints),
                    new XElement("MaxMagicPoints", MaxMagicPoints),
                    new XElement("MagicPoints", MagicPoints),
                    new XElement("Attack", Attack),
                    new XElement("Defense", Defense),
                    new XElement("Speed", Speed),
                    new XElement("Dexterity", Dexterity),
                    new XElement("HpRegen", HpRegen),
                    new XElement("MpRegen", MpRegen),
                    new XElement("Tex1", Tex1),
                    new XElement("Tex2", Tex2),
                    new XElement("PCStats", PCStats),
                    new XElement("Dead", Dead),
                    new XElement("Pet", Pet)
                );
        }
    }

    class CharList
    {
        public Character[] Characters { get; private set; }
        public int NextCharId { get; private set; }
        public int MaxNumChars { get; private set; }

        public Account Account { get; private set; }

        public IEnumerable<NewsItem> News { get; private set; }
        public IEnumerable<ServerItem> Servers { get; set; }

        public double? Lat { get; set; }
        public double? Long { get; set; }

        static IEnumerable<NewsItem> GetItems(Database db, DbAccount acc)
        {
            var news = new DbNews(db, 10).Entries
                .Select(x => NewsItem.FromDb(x)).ToArray();
            var chars = db.GetDeadCharacters(acc).Take(10).Select(x =>
            {
                var death = new DbDeath(acc, x);
                return new NewsItem()
                {
                    Icon = "fame",
                    Title = "Your " + Program.GameData.ObjectTypeToId[death.ObjectType]
                            + " died at level " + death.Level,
                    TagLine = "You earned " + death.TotalFame + " glorious Fame",
                    Link = "fame:" + death.CharId,
                    Date = death.DeathTime
                };
            });
            return news.Concat(chars).OrderByDescending(x => x.Date);
        }

        public static CharList FromDb(Database db, DbAccount acc)
        {
            return new CharList()
            {
                Characters = db.GetAliveCharacters(acc)
                                .Select(x => Character.FromDb(db.LoadCharacter(acc, x), false))
                                .ToArray(),
                NextCharId = acc.NextCharId,
                MaxNumChars = acc.MaxCharSlot,
                Account = Account.FromDb(acc),
                News = GetItems(db, acc)
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("Chars",
                    new XAttribute("nextCharId", NextCharId),
                    new XAttribute("maxNumChars", MaxNumChars),
                    Characters.Select(x => x.ToXml()),
                    Account.ToXml(),
                    new XElement("News",
                        News.Select(x => x.ToXml())
                    ),
                    new XElement("Servers",
                        Servers.Select(x => x.ToXml())
                    ),
                    Lat == null ? null : new XElement("Lat", Lat),
                    Long == null ? null : new XElement("Long", Long)
                );
        }
    }

    class Fame
    {
        public string Name { get; private set; }
        public Character Character { get; private set; }
        public FameStats Stats { get; private set; }
        public IEnumerable<Tuple<string, string, double>> Bonuses { get; private set; }
        public int TotalFame { get; private set; }

        public bool FirstBorn { get; private set; }
        public DateTime DeathTime { get; private set; }
        public string Killer { get; private set; }

        public static Fame FromDb(DbChar character)
        {
            DbDeath death = new DbDeath(character.Account, character.CharId);
            if (death.IsNull) return null;
            var stats = FameStats.Read(character.FameStats);
            return new Fame()
            {
                Name = character.Account.Name,
                Character = Character.FromDb(character, !death.IsNull),
                Stats = stats,
                Bonuses = stats.GetBonuses(Program.GameData, character, death.FirstBorn),
                TotalFame = death.TotalFame,

                FirstBorn = death.FirstBorn,
                DeathTime = death.DeathTime,
                Killer = death.Killer
            };
        }

        XElement GetCharElem()
        {
            var ret = Character.ToXml();
            ret.Add(new XElement("Account",
                new XElement("Name", Name)
            ));
            return ret;
        }

        public XElement ToXml()
        {
            return
                new XElement("Fame",
                    GetCharElem(),
                    new XElement("BaseFame", Character.CurrentFame),
                    new XElement("TotalFame", TotalFame),

                    new XElement("Shots", Stats.Shots),
                    new XElement("ShotsThatDamage", Stats.ShotsThatDamage),
                    new XElement("SpecialAbilityUses", Stats.SpecialAbilityUses),
                    new XElement("TilesUncovered", Stats.TilesUncovered),
                    new XElement("Teleports", Stats.Teleports),
                    new XElement("PotionsDrunk", Stats.PotionsDrunk),
                    new XElement("MonsterKills", Stats.MonsterKills),
                    new XElement("MonsterAssists", Stats.MonsterAssists),
                    new XElement("GodKills", Stats.GodKills),
                    new XElement("GodAssists", Stats.GodAssists),
                    new XElement("CubeKills", Stats.CubeKills),
                    new XElement("OryxKills", Stats.OryxKills),
                    new XElement("QuestsCompleted", Stats.QuestsCompleted),
                    new XElement("PirateCavesCompleted", Stats.PirateCavesCompleted),
                    new XElement("UndeadLairsCompleted", Stats.UndeadLairsCompleted),
                    new XElement("AbyssOfDemonsCompleted", Stats.AbyssOfDemonsCompleted),
                    new XElement("SnakePitsCompleted", Stats.SnakePitsCompleted),
                    new XElement("SpiderDensCompleted", Stats.SpiderDensCompleted),
                    new XElement("SpriteWorldsCompleted", Stats.SpriteWorldsCompleted),
                    new XElement("LevelUpAssists", Stats.LevelUpAssists),
                    new XElement("MinutesActive", Stats.MinutesActive),
                    new XElement("TombsCompleted", Stats.TombsCompleted),
                    new XElement("TrenchesCompleted", Stats.TrenchesCompleted),
                    new XElement("JunglesCompleted", Stats.JunglesCompleted),
                    new XElement("ManorsCompleted", Stats.ManorsCompleted),
                    Bonuses.Select(x =>
                        new XElement("Bonus",
                            new XAttribute("id", x.Item1),
                            new XAttribute("desc", x.Item2),
                            x.Item3
                        )
                    ),
                    new XElement("CreatedOn", DeathTime.ToUnixTimestamp()),
                    new XElement("KilledBy", Killer)
                );
        }
    }

    class FameListEntry
    {
        public int AccountId { get; private set; }
        public int CharId { get; private set; }
        public string Name { get; private set; }
        public ushort ObjectType { get; private set; }
        public int Tex1 { get; private set; }
        public int Tex2 { get; private set; }
        public ushort[] Equipment { get; private set; }
        public int TotalFame { get; private set; }

        public static FameListEntry FromDb(DbChar character)
        {
            var death = new DbDeath(character.Account, character.CharId);
            return new FameListEntry()
            {
                AccountId = character.Account.AccountId,
                CharId = character.CharId,
                Name = character.Account.Name,
                ObjectType = character.ObjectType,
                Tex1 = character.Tex1,
                Tex2 = character.Tex2,
                Equipment = character.Items,
                TotalFame = death.TotalFame
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("FameListElem",
                    new XAttribute("accountId", AccountId),
                    new XAttribute("charId", CharId),
                    new XElement("Name", Name),
                    new XElement("ObjectType", ObjectType),
                    new XElement("Tex1", Tex1),
                    new XElement("Tex2", Tex2),
                    new XElement("Equipment", Equipment.ToCommaSepString()),
                    new XElement("TotalFame", TotalFame)
                );
        }
    }
    class FameList
    {
        public string TimeSpan { get; private set; }
        public IEnumerable<FameListEntry> Entries { get; private set; }

        public static FameList FromDb(Database db, string timeSpan, DbChar character)
        {

            DbLegendTimeSpan span;
            if (timeSpan.EqualsIgnoreCase("week")) span = DbLegendTimeSpan.Week;
            else if (timeSpan.EqualsIgnoreCase("month")) span = DbLegendTimeSpan.Month;
            else if (timeSpan.EqualsIgnoreCase("all")) span = DbLegendTimeSpan.All;
            else return null;

            DbLegend legend = new DbLegend(db, span, 20);
            IEnumerable<DbChar> chars;
            if (character == null)
                chars = legend.Entries.Select(x =>
                    db.LoadCharacter(x.AccId, x.ChrId));
            else
                chars = legend.Entries.Select(x =>
                    db.LoadCharacter(x.AccId, x.ChrId)
                ).Concat(new[] { character }).Take(20);

            return new FameList()
            {
                TimeSpan = timeSpan.ToLower(),
                Entries = chars
                    .Select(x => FameListEntry.FromDb(x))
                    .OrderByDescending(x => x.TotalFame)
            };
        }

        public XElement ToXml()
        {
            return
                new XElement("FameList",
                    new XAttribute("timespan", TimeSpan),
                    Entries.Select(x => x.ToXml())
                );
        }
    }
}
