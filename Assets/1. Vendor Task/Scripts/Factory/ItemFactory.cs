using System.Collections.Generic;
using System.Linq;
using VendorTask.Items;

namespace VendorTask.Factory
{
    public class ItemFactory
    {
        private readonly Dictionary<int, ItemConfig> _configs;

        public ItemFactory(IEnumerable<ItemConfig> configs)
        {
            _configs = configs.ToDictionary(key => key.Id, value => value);
        }

        public Item Create(int itemId)
        {
            var config = _configs[itemId];
            return config.CreateInstance();
        }
    }
}
