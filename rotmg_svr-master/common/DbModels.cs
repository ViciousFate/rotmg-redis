using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using BookSleeve;

namespace common
{
    public abstract class RedisObject
    {
        //Note do not modify returning buffer
        Dictionary<string, KeyValuePair<byte[], bool>> fields;
        protected void Init(Database db, string key)
        {
            this.Key = key;
            this.Database = db;
            fields = db.Hashes.GetAll(0, key).Exec()
                .ToDictionary(
                    x => x.Key,
                    x => new KeyValuePair<byte[], bool>(x.Value, false));
        }

        public Database Database { get; private set; }
        public string Key { get; private set; }
        public IEnumerable<string> AllKeys { get { return fields.Keys; } }
        public bool IsNull { get { return fields.Count == 0; } }

        protected T GetValue<T>(string key, T def = default(T))
        {
            KeyValuePair<byte[], bool> val;
            if (!fields.TryGetValue(key, out val))
                return def;
            if (typeof(T) == typeof(int))
                return (T)(object)int.Parse(Encoding.UTF8.GetString(val.Key));

            else if (typeof(T) == typeof(ushort))
                return (T)(object)ushort.Parse(Encoding.UTF8.GetString(val.Key));

            else if (typeof(T) == typeof(bool))
                return (T)(object)(val.Key[0] != 0);

            else if (typeof(T) == typeof(DateTime))
                return (T)(object)DateTime.FromBinary(BitConverter.ToInt64(val.Key, 0));

            else if (typeof(T) == typeof(byte[]))
                return (T)(object)val.Key;

            else if (typeof(T) == typeof(ushort[]))
            {
                ushort[] ret = new ushort[val.Key.Length / 2];
                Buffer.BlockCopy(val.Key, 0, ret, 0, val.Key.Length);
                return (T)(object)ret;
            }

            else if (typeof(T) == typeof(int[]))
            {
                int[] ret = new int[val.Key.Length / 4];
                Buffer.BlockCopy(val.Key, 0, ret, 0, val.Key.Length);
                return (T)(object)ret;
            }

            else if (typeof(T) == typeof(string))
                return (T)(object)Encoding.UTF8.GetString(val.Key);

            else
                throw new NotSupportedException();
        }

        protected void SetValue<T>(string key, T val)
        {
            byte[] buff;
            if (typeof(T) == typeof(int) || typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(string))
                buff = Encoding.UTF8.GetBytes(val.ToString());

            else if (typeof(T) == typeof(bool))
                buff = new byte[] { (byte)((bool)(object)val ? 1 : 0) };

            else if (typeof(T) == typeof(DateTime))
                buff = BitConverter.GetBytes(((DateTime)(object)val).ToBinary());

            else if (typeof(T) == typeof(byte[]))
                buff = (byte[])(object)val;

            else if (typeof(T) == typeof(ushort[]))
            {
                var v = (ushort[])(object)val;
                buff = new byte[v.Length * 2];
                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }

            else if (typeof(T) == typeof(int[]))
            {
                var v = (int[])(object)val;
                buff = new byte[v.Length * 4];
                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }

            else
                throw new NotSupportedException();
            fields[key] = new KeyValuePair<byte[], bool>(buff, true);
        }

        Dictionary<string, byte[]> update;
        public void Flush()
        {
            if (update == null) update = new Dictionary<string, byte[]>();
            else update.Clear();
            foreach (var i in fields)
                if (i.Value.Value)
                    update.Add(i.Key, i.Value.Key);
            Database.Hashes.Set(0, Key, update);
        }
        public void Flush(RedisConnection conn = null)
        {
            if (update == null) update = new Dictionary<string, byte[]>();
            else update.Clear();
            foreach (var i in fields)
                if (i.Value.Value)
                    update.Add(i.Key, i.Value.Key);
            (conn ?? Database).Hashes.Set(0, Key, update);
        }

        public void Reload()    //Discard all updates
        {
            if (update != null)
                update.Clear();

            fields = Database.Hashes.GetAll(0, Key).Exec()
                .ToDictionary(
                    x => x.Key,
                    x => new KeyValuePair<byte[], bool>(x.Value, false));
        }
    }

    public class DbLoginInfo
    {
        Database db;
        internal DbLoginInfo(Database db, string uuid)
        {
            this.db = db;
            UUID = uuid;
            var json = db.Hashes.GetString(0, "logins", uuid.ToUpperInvariant()).Exec();
            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }
        [JsonIgnore]
        public string UUID { get; private set; }

        [JsonIgnore]
        public bool IsNull { get; private set; }

        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public int AccountId { get; set; }

        public void Flush()
        {
            db.Hashes.Set(0, "logins", UUID.ToUpperInvariant(), JsonConvert.SerializeObject(this));
        }
    }

    public class DbAccount : RedisObject
    {
        internal DbAccount(Database db, int accId)
        {
            AccountId = accId;
            Init(db, "account." + accId);
        }

        public int AccountId { get; private set; }

        internal string LockToken { get; set; }

        public string UUID
        {
            get { return GetValue<string>("uuid"); }
            set { SetValue<string>("uuid", value); }
        }
        public string Name
        {
            get { return GetValue<string>("name"); }
            set { SetValue<string>("name", value); }
        }
        public bool Admin
        {
            get { return GetValue<bool>("admin"); }
            set { SetValue<bool>("admin", value); }
        }
        public bool NameChosen
        {
            get { return GetValue<bool>("nameChosen"); }
            set { SetValue<bool>("nameChosen", value); }
        }
        public bool Verified
        {
            get { return GetValue<bool>("verified"); }
            set { SetValue<bool>("verified", value); }
        }
        public int GuildId
        {
            get { return GetValue<int>("guildId"); }
            set { SetValue<int>("guildId", value); }
        }
        public int GuildRank
        {
            get { return GetValue<int>("guildRank"); }
            set { SetValue<int>("guildRank", value); }
        }
        public int VaultCount
        {
            get { return GetValue<int>("vaultCount"); }
            set { SetValue<int>("vaultCount", value); }
        }
        public int MaxCharSlot
        {
            get { return GetValue<int>("maxCharSlot"); }
            set { SetValue<int>("maxCharSlot", value); }
        }
        public DateTime RegTime
        {
            get { return GetValue<DateTime>("regTime"); }
            set { SetValue<DateTime>("regTime", value); }
        }
        public bool Guest
        {
            get { return GetValue<bool>("guest"); }
            set { SetValue<bool>("guest", value); }
        }

        public int Fame
        {
            get { return GetValue<int>("fame"); }
            set { SetValue<int>("fame", value); }
        }
        public int TotalFame
        {
            get { return GetValue<int>("totalFame"); }
            set { SetValue<int>("totalFame", value); }
        }
        public int Credits
        {
            get { return GetValue<int>("credits"); }
            set { SetValue<int>("credits", value); }
        }
        public int TotalCredits
        {
            get { return GetValue<int>("totalCredits"); }
            set { SetValue<int>("totalCredits", value); }
        }

        public int NextCharId
        {
            get { return GetValue<int>("nextCharId"); }
            set { SetValue<int>("nextCharId", value); }
        }
    }

    public struct DbClassStatsEntry
    {
        public int BestLevel;
        public int BestFame;
    }
    public class DbClassStats : RedisObject
    {
        public DbAccount Account { get; private set; }
        public DbClassStats(DbAccount acc)
        {
            Account = acc;
            Init(acc.Database, "classStats." + acc.AccountId);
        }

        public void Update(DbChar character)
        {
            var field = character.ObjectType.ToString();
            string json = GetValue<string>(field);
            if (json == null)
                SetValue<string>(field, JsonConvert.SerializeObject(new DbClassStatsEntry()
                {
                    BestLevel = character.Level,
                    BestFame = character.Fame
                }));
            else
            {
                var entry = JsonConvert.DeserializeObject<DbClassStatsEntry>(json);
                if (character.Level > entry.BestLevel)
                    entry.BestLevel = character.Level;
                if (character.Fame > entry.BestFame)
                    entry.BestFame = character.Fame;
                SetValue<string>(field, JsonConvert.SerializeObject(entry));
            }
        }

        public DbClassStatsEntry this[ushort type]
        {
            get
            {
                string v = GetValue<string>(type.ToString());
                if (v != null) return JsonConvert.DeserializeObject<DbClassStatsEntry>(v);
                else return default(DbClassStatsEntry);
            }
            set { SetValue<string>(type.ToString(), JsonConvert.SerializeObject(value)); }
        }
    }

    public class DbChar : RedisObject
    {
        public DbAccount Account { get; private set; }
        public int CharId { get; private set; }
        internal DbChar(DbAccount acc, int charId)
        {
            Account = acc;
            CharId = charId;
            Init(acc.Database, "char." + acc.AccountId + "." + charId);
        }

        public ushort ObjectType
        {
            get { return GetValue<ushort>("charType"); }
            set { SetValue<ushort>("charType", value); }
        }
        public int Level
        {
            get { return GetValue<int>("level"); }
            set { SetValue<int>("level", value); }
        }
        public int Experience
        {
            get { return GetValue<int>("exp"); }
            set { SetValue<int>("exp", value); }
        }
        public int Fame
        {
            get { return GetValue<int>("fame"); }
            set { SetValue<int>("fame", value); }
        }
        public ushort[] Items
        {
            get { return GetValue<ushort[]>("items"); }
            set { SetValue<ushort[]>("items", value); }
        }
        public int HP
        {
            get { return GetValue<int>("hp"); }
            set { SetValue<int>("hp", value); }
        }
        public int MP
        {
            get { return GetValue<int>("mp"); }
            set { SetValue<int>("mp", value); }
        }
        public int[] Stats
        {
            get { return GetValue<int[]>("stats"); }
            set { SetValue<int[]>("stats", value); }
        }
        public int Tex1
        {
            get { return GetValue<int>("tex1"); }
            set { SetValue<int>("tex1", value); }
        }
        public int Tex2
        {
            get { return GetValue<int>("tex2"); }
            set { SetValue<int>("tex2", value); }
        }
        public ushort Pet
        {
            get { return GetValue<ushort>("pet"); }
            set { SetValue<ushort>("pet", value); }
        }
        public byte[] FameStats
        {
            get { return GetValue<byte[]>("fameStats"); }
            set { SetValue<byte[]>("fameStats", value); }
        }
        public DateTime CreateTime
        {
            get { return GetValue<DateTime>("createTime"); }
            set { SetValue<DateTime>("createTime", value); }
        }
        public DateTime LastSeen
        {
            get { return GetValue<DateTime>("lastSeen"); }
            set { SetValue<DateTime>("lastSeen", value); }
        }
        public bool Dead
        {
            get { return GetValue<bool>("dead"); }
            set { SetValue<bool>("dead", value); }
        }
    }

    public class DbDeath : RedisObject
    {
        public DbAccount Account { get; private set; }
        public int CharId { get; private set; }
        public DbDeath(DbAccount acc, int charId)
        {
            Account = acc;
            CharId = charId;
            Init(acc.Database, "death." + acc.AccountId + "." + charId);
        }

        public ushort ObjectType
        {
            get { return GetValue<ushort>("objType"); }
            set { SetValue<ushort>("objType", value); }
        }
        public int Level
        {
            get { return GetValue<int>("level"); }
            set { SetValue<int>("level", value); }
        }
        public int TotalFame
        {
            get { return GetValue<int>("totalFame"); }
            set { SetValue<int>("totalFame", value); }
        }
        public string Killer
        {
            get { return GetValue<string>("killer"); }
            set { SetValue<string>("killer", value); }
        }
        public bool FirstBorn
        {
            get { return GetValue<bool>("firstBorn"); }
            set { SetValue<bool>("firstBorn", value); }
        }
        public DateTime DeathTime
        {
            get { return GetValue<DateTime>("deathTime"); }
            set { SetValue<DateTime>("deathTime", value); }
        }
    }

    public struct DbNewsEntry
    {
        [JsonIgnore]
        public DateTime Date;
        public string Icon;
        public string Title;
        public string Text;
        public string Link;
    }
    public class DbNews
    {
        public DbNews(Database db, int count)
        {
            news = db.SortedSets.Range(0, "news", 0, 10, false).Exec()
                .Select(x =>
                {
                    DbNewsEntry ret = JsonConvert.DeserializeObject<DbNewsEntry>(
                        Encoding.UTF8.GetString(x.Key));
                    ret.Date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(x.Value);
                    return ret;
                }).ToArray();
        }

        DbNewsEntry[] news;
        public DbNewsEntry[] Entries { get { return news; } }
    }
    public class DbVault : RedisObject
    {
        public DbAccount Account { get; private set; }
        public DbVault(DbAccount acc)
        {
            Account = acc;
            Init(acc.Database, "vault." + acc.AccountId);
        }

        public ushort[] this[int index]
        {
            get { return GetValue<ushort[]>("vault." + index); }
            set { SetValue<ushort[]>("vault." + index, value); }
        }
    }

    public struct DbLegendEntry
    {
        public int TotalFame;

        public int AccId;
        public int ChrId;
    }
    public enum DbLegendTimeSpan
    {
        All,
        Month,
        Week
    }
    public class DbLegend
    {
        public DbLegend(Database db, DbLegendTimeSpan timeSpan, int count)
        {
            double begin;
            if (timeSpan == DbLegendTimeSpan.Week)
                begin = DateTime.Now.Subtract(TimeSpan.FromDays(7)).ToUnixTimestamp();
            else if (timeSpan == DbLegendTimeSpan.Month)
                begin = DateTime.Now.AddMonths(-1).ToUnixTimestamp();
            else
                begin = 0;

            entries = db.SortedSets.Range(0, "legends", begin, double.PositiveInfinity, false, count: count).Exec()
                .Select(x => new DbLegendEntry()
                {
                    TotalFame = BitConverter.ToInt32(x.Key, 0),
                    AccId = BitConverter.ToInt32(x.Key, 4),
                    ChrId = BitConverter.ToInt32(x.Key, 8)
                })
                .OrderByDescending(x => x.TotalFame)
                .ToArray();
        }

        public static void Insert(Database db, DateTime time, DbLegendEntry entry)
        {
            double t = time.ToUnixTimestamp();
            byte[] buff = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(entry.TotalFame), 0, buff, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(entry.AccId), 0, buff, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(entry.ChrId), 0, buff, 8, 4);
            db.SortedSets.Add(0, "legends", buff, t);
        }

        DbLegendEntry[] entries;
        public DbLegendEntry[] Entries { get { return entries; } }
    }
}
