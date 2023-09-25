using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VendorTask.Items
{
    public class InfiniteInventory : IInventory
    {
        private readonly List<Item> _itemsCollection;

        public InfiniteInventory()
        {
            _itemsCollection = new List<Item>();
        }

        public InfiniteInventory(IEnumerable<Item> itemsCollection)
        {
            _itemsCollection = itemsCollection.ToList();
        }

        public void Add(Item item)
        {
            _itemsCollection.Add(item);
        }

        public void Remove(Item item)
        {
            if (Contains(item.Id) == false)
                throw new InvalidOperationException(nameof(item));

            _itemsCollection.Remove(item);
        }

        public bool Contains(int id)
        {
            return _itemsCollection.Exists(item => item.Id == id);
        }

        public Item Get(int id)
        {
            if (Contains(id) == false)
                throw new ArgumentException(nameof(id));

            return _itemsCollection.First(item => item.Id == id); ;
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _itemsCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}