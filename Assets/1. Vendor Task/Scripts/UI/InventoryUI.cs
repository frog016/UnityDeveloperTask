using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VendorTask.Factory;
using VendorTask.Items;

namespace VendorTask.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform _viewRoot;
        [SerializeField] private RectTransform _layoutGroup;

        public event Action<ItemSlot, ItemView> ItemDroppedInSlot; 

        private UIFactory _uiFactory;
        private ItemSlot[] _slots;

        public void Initialize(UIFactory uiFactory, IEnumerable<Item> items)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup);

            _uiFactory = uiFactory;
            _slots = FindSlotsInChildren();
            InitializeSlotsWithItems(items);

            foreach (var slot in _slots)
                slot.ItemDropped += InvokeItemDroppedEvent;
        }

        private void OnDestroy()
        {
            foreach (var slot in _slots)
                slot.ItemDropped -= InvokeItemDroppedEvent;
        }

        public bool ContainsContent(ItemView view)
        {
            return _slots.FirstOrDefault(slot => slot.Content == view) != null;
        }

        public bool Contains(ItemSlot slot)
        {
            return _slots.Contains(slot);
        }

        private ItemSlot[] FindSlotsInChildren()
        {
            return GetComponentsInChildren<ItemSlot>()
                .ToArray();
        }

        private void InitializeSlotsWithItems(IEnumerable<Item> items)
        {
            var slotItemPairs = _slots.Zip(items, Tuple.Create);
            foreach (var (slot, item) in slotItemPairs)
            {
                var itemView = _uiFactory.CreateItemView(item, _viewRoot);
                slot.Initialize(itemView);
            }
        }

        private void InvokeItemDroppedEvent(ItemSlot slot, ItemView view)
        {
            ItemDroppedInSlot?.Invoke(slot, view);
        }
    }
}