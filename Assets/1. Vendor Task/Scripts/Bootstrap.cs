using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VendorTask.Factory;
using VendorTask.Items;
using VendorTask.UI;

namespace VendorTask
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private InventoryUI _playerInventoryUI;
        [SerializeField] private ItemView _itemViewPrefab;
        [SerializeField] private ItemConfig[] _itemConfigs;

        [Header("Data")] 
        [SerializeField] private int[] _playerItemIds;

        private void Start()
        {
            var uiFactory = new UIFactory(_itemViewPrefab);
            var itemFactory = new ItemFactory(_itemConfigs);

            InitializePersonInventoryUI(_playerInventoryUI, itemFactory, uiFactory, _playerItemIds);
        }

        private static void InitializePersonInventoryUI(InventoryUI inventoryUI, ItemFactory itemFactory, UIFactory uiFactory, IEnumerable<int> ids)
        {
            var items = CreateItemsById(itemFactory, ids).ToArray();
            inventoryUI.Initialize(uiFactory, items);
        }

        private static IEnumerable<Item> CreateItemsById(ItemFactory factory, IEnumerable<int> ids)
        {
            return ids.Select(factory.Create);
        }
    }
}