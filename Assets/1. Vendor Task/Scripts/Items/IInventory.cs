using System.Collections.Generic;

namespace VendorTask.Items
{
    public interface IInventory : IEnumerable<Item>
    {
        void Add(Item item);
        void Remove(Item item);
        bool Contains(int id);
        Item Get(int id);
    }
}