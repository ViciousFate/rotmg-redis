using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;

namespace wServer.realm
{
    public interface IContainer
    {
        int[] SlotTypes { get; }
        Inventory Inventory { get; }
    }

    public class InventoryChangedEventArgs : EventArgs
    {
        //index = -1 -> reset
        public InventoryChangedEventArgs(int index, Item old, Item @new)
        {
            Index = index;
            OldItem = old;
            NewItem = @new;
        }

        public int Index { get; private set; }
        public Item OldItem { get; private set; }
        public Item NewItem { get; private set; }
    }
    public class Inventory : IEnumerable<Item>
    {
        Item[] items;
        IContainer parent;

        public Inventory(IContainer parent) : this(parent, new Item[12]) { }
        public Inventory(IContainer parent, Item[] items)
        {
            this.parent = parent;
            this.items = items;
        }

        public IContainer Parent { get; private set; }
        public int Length { get { return items.Length; } }

        public void SetItems(Item[] items)
        {
            this.items = items;
            if (InventoryChanged != null)
                InventoryChanged(this, new InventoryChangedEventArgs(-1, null, null));
        }

        public Item this[int index]
        {
            get { return items[index]; }
            set
            {
                if (items[index] != value)
                {
                    var e = new InventoryChangedEventArgs(index, items[index], value);
                    items[index] = value;
                    if (InventoryChanged != null)
                        InventoryChanged(this, e);
                }
            }
        }

        public event EventHandler<InventoryChangedEventArgs> InventoryChanged;

        public IEnumerator<Item> GetEnumerator()
        {
            return ((IEnumerable<Item>)items).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
