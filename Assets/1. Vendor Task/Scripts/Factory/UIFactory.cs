using UnityEngine;
using VendorTask.Items;
using VendorTask.UI;

namespace VendorTask.Factory
{
    public class UIFactory
    {
        private readonly ItemView _itemViewPrefab;

        public UIFactory(ItemView itemViewPrefab)
        {
            _itemViewPrefab = itemViewPrefab;
        }

        public ItemView CreateItemView(Item item, Transform parent)
        {
            var instance = Object.Instantiate(_itemViewPrefab, parent);
            instance.Initialize(item);

            return instance;
        }
    }
}