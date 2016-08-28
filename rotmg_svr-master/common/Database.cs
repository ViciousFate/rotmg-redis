using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Ionic.Zlib;
using System.Xml.Linq;
using log4net;
using BookSleeve;
using System.Security.Cryptography;

namespace common
{
    public class Database : RedisConnection
    {
        static ILog log = LogManager.GetLogger(typeof(Database));

        public Database(string host, int port, string password)
            : base(host, port, password: password == "" ? null : password)
        {
            SetKeepAlive(60);
            Open().Wait();
        }

        static string[] names = new string[] { 
            "Darq", "Deyst", "Drac", "Drol",
            "Eango", "Eashy", "Eati", "Eendi", "Ehoni",
            "Gharr", "Iatho", "Iawa", "Idrae", "Iri", "Issz", "Itani",
            "Laen", "Lauk", "Lorz",
            "Oalei", "Odaru", "Oeti", "Orothi", "Oshyu",
            "Queq", "Radph", "Rayr", "Ril", "Rilr", "Risrr",
            "Saylt", "Scheev", "Sek", "Serl", "Seus",
            "Tal", "Tiar", "Uoro", "Urake", "Utanu",
            "Vorck", "Vorv", "Yangu", "Yimi", "Zhiar"
        };
        public DbAccount CreateGuestAccount(string uuid)
        {
            return new DbAccount(this, 0)
            {
                UUID = uuid,
                Name = names[(uint)uuid.GetHashCode() % names.Length],
                Admin = false,
                NameChosen = false,
                Verified = false,
                GuildId = -1,
                GuildRank = -1,
                VaultCount = 1,
                MaxCharSlot = 1,
                RegTime = DateTime.Now,
                Guest = true,

                Fame = 0,
                TotalFame = 0,
                Credits = 1000,
                TotalCredits = 0
            };
        }

        public LoginStatus Verify(string uuid, string password, out DbAccount acc)
        {
            acc = null;

            //check login
            DbLoginInfo info = new DbLoginInfo(this, uuid);
            if (info.IsNull)
                return LoginStatus.AccountNotExists;

            byte[] userPass = Utils.SHA1(password + info.Salt);
            if (Convert.ToBase64String(userPass) != info.HashedPassword)
                return LoginStatus.InvalidCredentials;

            acc = new DbAccount(this, info.AccountId);

            return LoginStatus.OK;
        }

        public bool AcquireLock(DbAccount acc)
        {
            string lockToken = Guid.NewGuid().ToString();
            string key = "lock." + acc.AccountId;
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyNotExists(1, key));

                trans.Strings.Set(1, key, lockToken);
                trans.Keys.Expire(1, key, 60);

                bool ok = trans.Execute().Exec();
                acc.LockToken = ok ? lockToken : null;
                return ok;
            }
        }
        public int GetLockTime(DbAccount acc)
        {
            return (int)Keys.TimeToLive(1, "lock." + acc.AccountId).Exec();
        }
        public int GetLockTime(int id)
        {
            return (int)Keys.TimeToLive(1, "lock." + id).Exec();
        }

        public bool RenewLock(DbAccount acc)
        {
            string key = "lock." + acc.AccountId;
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, key, acc.LockToken));
                Keys.Expire(1, key, 60);
                return trans.Execute().Exec();
            }
        }
        public void ReleaseLock(DbAccount acc)
        {
            string key = "lock." + acc.AccountId;
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, key, acc.LockToken));
                trans.Keys.Remove(1, key);
                trans.Execute().Exec();
            }
        }

        public IDisposable Lock(DbAccount acc)
        {
            return new l(this, acc);
        }
        public bool LockOk(IDisposable l)
        {
            return ((l)l).lockOk;
        }
        struct l : IDisposable
        {
            Database db;
            DbAccount acc;
            internal bool lockOk;
            public l(Database db, DbAccount acc)
            {
                this.db = db;
                this.acc = acc;
                lockOk = db.AcquireLock(acc);
            }

            public void Dispose()
            {
                if (lockOk)
                    db.ReleaseLock(acc);
            }
        }

        public const string REG_LOCK = "regLock";
        public const string NAME_LOCK = "nameLock";

        public string AcquireLock(string key)
        {
            string lockToken = Guid.NewGuid().ToString();
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyNotExists(1, key));

                trans.Strings.Set(1, key, lockToken);
                trans.Keys.Expire(1, key, 60);

                return trans.Execute().Exec() ? lockToken : null;
            }
        }
        public void ReleaseLock(string key, string token)
        {
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, key, token));
                trans.Keys.Remove(1, key);
                trans.Execute();
            }
        }

        public bool RenameUUID(DbAccount acc, string newUuid, string lockToken)
        {
            string p = Hashes.GetString(0, "login", acc.UUID.ToUpperInvariant()).Exec();
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, REG_LOCK, lockToken));
                trans.Hashes.Remove(0, "login", acc.UUID.ToUpperInvariant());
                trans.Hashes.Set(0, "login", newUuid.ToUpperInvariant(), p);
                if (!trans.Execute().Exec()) return false;
            }
            acc.UUID = newUuid;
            acc.Flush();
            return true;
        }
        public bool RenameIGN(DbAccount acc, string newName, string lockToken)
        {
            if (names.Contains(newName, StringComparer.InvariantCultureIgnoreCase))
                return false;
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, NAME_LOCK, lockToken));
                Hashes.Remove(0, "names", acc.Name.ToUpperInvariant());
                Hashes.Set(0, "names", newName.ToUpperInvariant(), acc.AccountId.ToString());
                if (!trans.Execute().Exec()) return false;
            }
            acc.Name = newName;
            acc.NameChosen = true;
            acc.Flush();
            return true;
        }

        static RandomNumberGenerator gen = RNGCryptoServiceProvider.Create();
        public void ChangePassword(string uuid, string password)
        {
            DbLoginInfo login = new DbLoginInfo(this, uuid);

            byte[] x = new byte[0x10];
            gen.GetNonZeroBytes(x);
            string salt = Convert.ToBase64String(x);
            string hash = Convert.ToBase64String(Utils.SHA1(password + salt));

            login.HashedPassword = hash;
            login.Salt = salt;
            login.Flush();
        }

        public RegisterStatus Register(string uuid, string password, bool isGuest, out DbAccount acc)
        {
            acc = null;
            if (!Hashes.SetIfNotExists(0, "logins", uuid.ToUpperInvariant(), "{}").Exec())
                return RegisterStatus.UsedName;

            int newAccId = (int)Strings.Increment(0, "nextAccId").Exec();

            acc = new DbAccount(this, newAccId)
            {
                UUID = uuid,
                Name = names[(uint)uuid.GetHashCode() % names.Length],
                Admin = false,
                NameChosen = false,
                Verified = false,
                GuildId = -1,
                GuildRank = -1,
                VaultCount = 1,
                MaxCharSlot = 1,
                RegTime = DateTime.Now,
                Guest = true,
                Fame = 0,
                TotalFame = 0,
                Credits = 1000,
                TotalCredits = 0
            };
            acc.Flush();

            DbLoginInfo login = new DbLoginInfo(this, uuid);

            byte[] x = new byte[0x10];
            gen.GetNonZeroBytes(x);
            string salt = Convert.ToBase64String(x);
            string hash = Convert.ToBase64String(Utils.SHA1(password + salt));

            login.HashedPassword = hash;
            login.Salt = salt;
            login.AccountId = acc.AccountId;
            login.Flush();

            DbClassStats stats = new DbClassStats(acc);
            stats.Flush();

            DbVault vault = new DbVault(acc);
            vault[0] = Enumerable.Repeat((ushort)0xffff, 8).ToArray();
            vault.Flush();

            return RegisterStatus.OK;
        }

        public bool HasUUID(string uuid)
        {
            return Hashes.Exists(0, "login", uuid.ToUpperInvariant()).Exec();
        }
        public DbAccount GetAccount(int id)
        {
            var ret = new DbAccount(this, id);
            if (ret.IsNull) return null;
            return ret;
        }
        public DbAccount GetAccount(string uuid)
        {
            DbLoginInfo info = new DbLoginInfo(this, uuid);
            if (info.IsNull)
                return null;
            DbAccount ret = new DbAccount(this, info.AccountId);
            if (ret.IsNull)
                return null;
            return ret;
        }

        public int ResolveId(string ign)
        {
            string val = Hashes.GetString(0, "names", ign.ToUpperInvariant()).Exec();
            if (val == null) return 0;
            else return int.Parse(val);
        }
        public string ResolveIgn(int accId)
        {
            return Hashes.GetString(0, "account." + accId, "name").Exec();
        }

        public void UpdateCredit(DbAccount acc, int amount)
        {
            if (amount > 0)
                WaitAll(
                    Hashes.Increment(0, acc.Key, "totalCredits", amount),
                    Hashes.Increment(0, acc.Key, "credits", amount));
            else
                Hashes.Increment(0, acc.Key, "credits", amount).Wait();
            acc.Flush();
            acc.Reload();
        }
        public void UpdateFame(DbAccount acc, int amount)
        {
            if (amount > 0)
                WaitAll(
                    Hashes.Increment(0, acc.Key, "totalFame", amount),
                    Hashes.Increment(0, acc.Key, "fame", amount));
            else
                Hashes.Increment(0, acc.Key, "fame", amount).Wait();
            acc.Flush();
            acc.Reload();
        }

        public DbClassStats ReadClassStats(DbAccount acc)
        {
            return new DbClassStats(acc);
        }

        public DbVault ReadVault(DbAccount acc)
        {
            return new DbVault(acc);
        }

        public int CreateChest(DbVault vault)
        {
            int id = (int)Hashes.Increment(0, vault.Account.Key, "vaultCount").Exec();
            vault[id] = Enumerable.Repeat((ushort)0xffff, 8).ToArray();
            vault.Flush();
            return id;
        }

        public IEnumerable<int> GetAliveCharacters(DbAccount acc)
        {
            foreach (var i in Sets.GetAll(0, "alive." + acc.AccountId).Exec())
                yield return BitConverter.ToInt32(i, 0);
        }
        public IEnumerable<int> GetDeadCharacters(DbAccount acc)
        {
            foreach (var i in Lists.Range(0, "dead." + acc.AccountId, 0, int.MaxValue).Exec())
                yield return BitConverter.ToInt32(i, 0);
        }
        public bool IsAlive(DbChar character)
        {
            return Sets.Contains(0, "alive." + character.Account.AccountId,
                                 BitConverter.GetBytes(character.CharId)).Exec();
        }

        public CreateStatus CreateCharacter(
            XmlData dat, DbAccount acc, ushort type, out DbChar character)
        {
            XElement cls = dat.ObjectTypeToElement[type];

            if (Sets.GetLength(0, "alive." + acc.AccountId).Exec() >= acc.MaxCharSlot)
            {
                character = null;
                return CreateStatus.ReachCharLimit;
            }

            int newId = (int)Hashes.Increment(0, acc.Key, "nextCharId").Exec();
            character = new DbChar(acc, newId)
            {
                ObjectType = type,
                Level = 1,
                Experience = 0,
                Fame = 0,
                Items = cls.Element("Equipment").Value.CommaToArray<ushort>(),
                Stats = new int[]{
                    ushort.Parse(cls.Element("MaxHitPoints").Value),
                    ushort.Parse(cls.Element("MaxMagicPoints").Value),
                    ushort.Parse(cls.Element("Attack").Value),
                    ushort.Parse(cls.Element("Defense").Value),
                    ushort.Parse(cls.Element("Speed").Value),
                    ushort.Parse(cls.Element("Dexterity").Value),
                    ushort.Parse(cls.Element("HpRegen").Value),
                    ushort.Parse(cls.Element("MpRegen").Value),
                },
                HP = int.Parse(cls.Element("MaxHitPoints").Value),
                MP = int.Parse(cls.Element("MaxMagicPoints").Value),
                Tex1 = 0,
                Tex2 = 0,
                Pet = 0xffff,
                FameStats = new byte[0],
                CreateTime = DateTime.Now,
                LastSeen = DateTime.Now
            };
            character.Flush();
            Sets.Add(0, "alive." + acc.AccountId, BitConverter.GetBytes(newId));
            return CreateStatus.OK;
        }

        public DbChar LoadCharacter(DbAccount acc, int charId)
        {
            DbChar ret = new DbChar(acc, charId);
            if (ret.IsNull) return null;
            else return ret;
        }
        public DbChar LoadCharacter(int accId, int charId)
        {
            DbAccount acc = new DbAccount(this, accId);
            if (acc.IsNull) return null;
            DbChar ret = new DbChar(acc, charId);
            if (ret.IsNull) return null;
            else return ret;
        }

        public bool SaveCharacter(DbAccount acc, DbChar character, bool lockAcc)
        {
            using (var trans = CreateTransaction())
            {
                if (lockAcc)
                    trans.AddCondition(Condition.KeyEquals(1,
                        "lock." + acc.AccountId, acc.LockToken));
                character.Flush(trans);
                DbClassStats stats = new DbClassStats(acc);
                stats.Update(character);
                stats.Flush(trans);
                return trans.Execute().Exec();
            }
        }
        public void DeleteCharacter(DbAccount acc, int charId)
        {
            Keys.Remove(0, "char." + acc.AccountId + "." + charId);
            var buff = BitConverter.GetBytes(charId);
            Sets.Remove(0, "alive." + acc.AccountId, buff);
            Lists.Remove(0, "dead." + acc.AccountId, buff);
        }

        public void Death(XmlData dat, DbAccount acc, DbChar character, FameStats stats, string killer)
        {
            character.Dead = true;
            SaveCharacter(acc, character, acc.LockToken != null);
            bool firstBorn;
            var finalFame = stats.CalculateTotal(dat, character,
                                new DbClassStats(acc), out firstBorn);

            DbDeath death = new DbDeath(acc, character.CharId);
            death.ObjectType = character.ObjectType;
            death.Level = character.Level;
            death.TotalFame = finalFame;
            death.Killer = killer;
            death.FirstBorn = firstBorn;
            death.DeathTime = DateTime.Now;
            death.Flush();

            byte[] idBuff = BitConverter.GetBytes(character.CharId);
            Sets.Remove(0, "alive." + acc.AccountId, idBuff);
            Lists.AddFirst(0, "dead." + acc.AccountId, idBuff);

            UpdateFame(acc, finalFame);

            DbLegendEntry entry = new DbLegendEntry()
            {
                AccId = acc.AccountId,
                ChrId = character.CharId,
                TotalFame = finalFame
            };
            DbLegend.Insert(this, death.DeathTime, entry);
        }

    }
}
