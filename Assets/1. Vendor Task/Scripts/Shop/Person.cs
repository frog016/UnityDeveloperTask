using VendorTask.Items;
using VendorTask.Shop.Wallet;

namespace VendorTask.Shop
{
    public class Person
    {
        public readonly IWallet Wallet;
        public readonly IInventory Inventory;

        public Person(IWallet wallet, IInventory inventory)
        {
            Wallet = wallet;
            Inventory = inventory;
        }
    }
}