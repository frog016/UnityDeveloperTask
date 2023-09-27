using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VendorTask.Factory;
using VendorTask.Items;
using VendorTask.Shop;
using VendorTask.Shop.Operation;
using VendorTask.Shop.Wallet;
using VendorTask.UI;

namespace VendorTask
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private InventoryUI _playerInventoryUI;
        [SerializeField] private InventoryUI _vendorInventoryUI;

        [Header("Item Data")]
        [SerializeField] private ItemView _itemViewPrefab;
        [SerializeField] private ItemConfig[] _itemConfigs;

        [Header("Inventory Data")]
        [SerializeField] private int[] _playerItemIds;
        [SerializeField] private int[] _vendorItemIds;

        [Header("Wallet Data")]
        [SerializeField] private int _playerInitialWalletBalance;
        [SerializeField] private WalletView _playerWalletView;

        private InventoryInputController _inventoryInputController;
        private WalletPresenter _walletPresenter;

        private void Start()
        {
            var vendorShop = CreateVendorShop();

            var uiFactory = new UIFactory(_itemViewPrefab);
            InitializePersonInventoryUI(_playerInventoryUI, uiFactory, vendorShop.CustomerPerson.Inventory);
            InitializePersonInventoryUI(_vendorInventoryUI, uiFactory, vendorShop.VendorPerson.Inventory);

            var shopOperations = CreateShopOperations(vendorShop);
            _inventoryInputController = new InventoryInputController(_playerInventoryUI, _vendorInventoryUI, shopOperations);
            _inventoryInputController.Initialize();

            _walletPresenter = new WalletPresenter(vendorShop.CustomerPerson.Wallet, _playerWalletView);
            _walletPresenter.Initialize();
        }

        private void OnDestroy()
        {
            _inventoryInputController.Dispose();
            _walletPresenter.Dispose();
        }

        private VendorShop CreateVendorShop()
        {
            var itemFactory = new ItemFactory(_itemConfigs);

            var vendorInventory = new InfiniteInventory(CreateItemsById(itemFactory, _vendorItemIds));
            var vendorPerson = new Person(new InfiniteWallet(), vendorInventory);

            var playerInventory = new InfiniteInventory(CreateItemsById(itemFactory, _playerItemIds));
            var playerPerson = new Person(new CommonWallet(_playerInitialWalletBalance), playerInventory);

            return new VendorShop(vendorPerson, playerPerson);
        }

        private static void InitializePersonInventoryUI(InventoryUI inventoryUI, UIFactory uiFactory, IEnumerable<Item> items)
        {
            inventoryUI.Initialize(uiFactory, items);
        }

        private static IEnumerable<Item> CreateItemsById(ItemFactory factory, IEnumerable<int> ids)
        {
            return ids.Select(factory.Create);
        }

        private static IEnumerable<IOperation> CreateShopOperations(VendorShop vendorShop)
        {
            yield return new ReplaceItemOperation();
            yield return new BuyVendorItemOperation(vendorShop);
            yield return new SellItemToVendorOperation(vendorShop);
        }
    }
}