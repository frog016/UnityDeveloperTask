using UnityEngine;
using VendorTask.Items;

namespace VendorTask.Shop
{
    public class VendorShop
    {
        private readonly Person _vendorPerson;
        private readonly Person _customerPerson;

        private const float BuyCostPercent = 0.9f; 

        public VendorShop(Person vendorPerson, Person customerPerson)
        {
            _vendorPerson = vendorPerson;
            _customerPerson = customerPerson;
        }

        public bool Buy(Item item)
        {
            var cost = GetVendorCost(item);
            return TryMakeDeal(_vendorPerson, _customerPerson, item, cost);
        }

        public bool Sell(Item item)
        {
            return TryMakeDeal(_customerPerson, _vendorPerson, item, item.Cost);
        }

        public int GetVendorCost(Item item)
        {
            return Mathf.RoundToInt(item.Cost * BuyCostPercent);
        }

        private static bool TryMakeDeal(Person first, Person second, Item item, int finalCost)
        {
            if (first.Wallet.IsEnough(item.Id) == false)
                return false;

            first.Wallet.Spend(finalCost);
            second.Wallet.Add(finalCost);

            first.Inventory.Add(item);
            second.Inventory.Remove(item);

            return true;
        }
    }
}
