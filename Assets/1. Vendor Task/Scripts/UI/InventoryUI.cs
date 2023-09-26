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

        public event Action<ItemView> ItemDroppedIntoSlot; 

        private UIFactory _uiFactory;
        private ItemSlot[] _slots;

        public void Initialize(UIFactory uiFactory, IEnumerable<Item> items)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup);

            _uiFactory = uiFactory;
            _slots = FindSlotsInChildren();
            InitializeSlotsWithItems(items);
        }

        private void OnDestroy()
        {
            foreach (var slot in _slots)
                slot.ItemDropped -= ItemDroppedIntoSlot;
        }

        public void AddItem(ItemView itemView)
        {
            var emptySlot = _slots.First(slot => slot.IsEmpty);
            emptySlot.Initialize(itemView);
        }

        public void RemoveItem(ItemView itemView)
        {
            var slot = _slots.First(slot => slot.Content == itemView);
            slot.RemoveContent();
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

                slot.ItemDropped += ItemDroppedIntoSlot;
            }
        }
    }
}