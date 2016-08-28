using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using common;
using wServer.realm.entities;
using System.Collections.Concurrent;
using wServer.networking;
using wServer.realm.terrain;

namespace wServer.realm.worlds
{
    public class Vault : World
    {
        Client client;
        public Vault(bool isLimbo, Client client = null)
        {
            Id = VAULT_ID;
            Name = "Vault";
            Background = 2;
            IsLimbo = isLimbo;

            this.client = client;
        }

        protected override void Init()
        {
            if (!IsLimbo)
            {
                base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.vault.wmap"));
                InitVault();
            }
        }

        DbVault dbVault;
        ConcurrentDictionary<Tuple<Container, int>, int> vaultChests = new ConcurrentDictionary<Tuple<Container, int>, int>();
        void InitVault()
        {
            List<IntPoint> vaultChestPosition = new List<IntPoint>();
            IntPoint spawn = new IntPoint(0, 0);

            int w = Map.Width;
            int h = Map.Height;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    var tile = Map[x, y];
                    if (tile.Region == TileRegion.Spawn)
                        spawn = new IntPoint(x, y);
                    else if (tile.Region == TileRegion.Vault)
                        vaultChestPosition.Add(new IntPoint(x, y));
                }
            vaultChestPosition.Sort((x, y) => Comparer<int>.Default.Compare(
                (x.X - spawn.X) * (x.X - spawn.X) + (x.Y - spawn.Y) * (x.Y - spawn.Y),
                (y.X - spawn.X) * (y.X - spawn.X) + (y.Y - spawn.Y) * (y.Y - spawn.Y)));

            dbVault = new DbVault(client.Account);
            for (int i = 0; i < client.Account.VaultCount; i++)
            {
                Container con = new Container(client.Manager, 0x0504, null, false);
                var inv = dbVault[i].Select(x => x == 0xffff ? null : client.Manager.GameData.Items[x]).ToArray();
                for (int j = 0; j < 8; j++)
                    con.Inventory[j] = inv[j];
                con.Move(vaultChestPosition[0].X + 0.5f, vaultChestPosition[0].Y + 0.5f);
                EnterWorld(con);
                vaultChestPosition.RemoveAt(0);

                vaultChests[Tuple.Create(con, i)] = con.UpdateCount;
            }
            foreach (var i in vaultChestPosition)
            {
                SellableObject x = new SellableObject(client.Manager, 0x0505);
                x.Move(i.X + 0.5f, i.Y + 0.5f);
                EnterWorld(x);
            }
        }

        public void AddChest(Entity original)
        {
            Container con = new Container(client.Manager, 0x0504, null, false);
            int index = Manager.Database.CreateChest(dbVault);
            var inv = dbVault[index].Select(x => x == 0xffff ? null : client.Manager.GameData.Items[x]).ToArray();
            for (int j = 0; j < 8; j++)
                con.Inventory[j] = inv[j];
            con.Move(original.X, original.Y);
            LeaveWorld(original);
            EnterWorld(con);

            vaultChests[Tuple.Create(con, index)] = con.UpdateCount;
        }

        public override World GetInstance(Client client)
        {
            return Manager.AddWorld(new Vault(false, client));
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            foreach (var i in vaultChests)
            {
                if (i.Key.Item1.UpdateCount > i.Value)
                {
                    dbVault[i.Key.Item2] = i.Key.Item1.Inventory.Take(8).Select(_ => _ == null ? (ushort)0xffff : _.ObjectType).ToArray();
                    dbVault.Flush();
                    vaultChests[i.Key] = i.Key.Item1.UpdateCount;
                }
            }

        }
    }
}
