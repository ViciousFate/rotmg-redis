using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using common;

namespace wServer.realm.entities
{
    public class Container : StaticObject, IContainer
    {
        public Container(RealmManager manager, ushort objType, int? life, bool dying)
            : base(manager, objType, life, false, dying, false)
        {
            Inventory = new Inventory(this);
            SlotTypes = new int[12];
            BagOwners = new int[0];

            var node = manager.GameData.ObjectTypeToElement[ObjectType];
            SlotTypes = node.Element("SlotTypes").Value.CommaToArray<int>();
            XElement eq = node.Element("Equipment");
            if (eq != null)
            {
                var inv = eq.Value.CommaToArray<ushort>().Select(_ => _ == 0xffff ? null : manager.GameData.Items[_]).ToArray();
                Array.Resize(ref inv, 12);
                Inventory.SetItems(inv);
            }
        }

        public Container(RealmManager manager, ushort id)
            : base(manager, id, null, false, false, false)
        {
            BagOwners = new int[0];
        }

        public int[] SlotTypes { get; private set; }
        public Inventory Inventory { get; private set; }
        public int[] BagOwners { get; set; }

        protected override void ImportStats(StatsType stats, object val)
        {
            switch (stats)
            {
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
                case StatsType.OwnerAccountId: break;// BagOwner = (int)val == -1 ? (int?)null : (int)val; break;
            }
            base.ImportStats(stats, val);
        }
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
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
            stats[StatsType.OwnerAccountId] = BagOwners.Length == 1 ? BagOwners[0] : -1;
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            if (ObjectType == 0x504)    //Vault chest
                return;
            bool hasItem = false;
            foreach (var i in Inventory)
                if (i != null)
                {
                    hasItem = true;
                    break;
                }
            if (!hasItem)
                Owner.LeaveWorld(this);
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }
    }
}
