using System;
using UnityEngine;
using VendorTask.Items;

namespace VendorTask.Shop
{
    public class VendorShop
    {
        public readonly Person VendorPerson;
        public readonly Person CustomerPerson;

        private const float BuyCostPercent = 0.9f; 

        public VendorShop(Person vendorPerson, Person customerPerson)
        {
            VendorPerson = vendorPerson;
            CustomerPerson = customerPerson;
        }

        public void Buy(Item item)
        {
            var cost = GetVendorCost(item);
            MakeDeal(VendorPerson, CustomerPerson, item, cost);
        }

        public void Sell(Item item)
        {
            MakeDeal(CustomerPerson, VendorPerson, item, item.Cost);
        }

        public int GetVendorCost(Item item)
        {
            return Mathf.RoundToInt(item.Cost * BuyCostPercent);
        }

        private static void MakeDeal(Person first, Person second, Item item, int finalCost)
        {
            if (first.Wallet.IsEnough(finalCost) == false)
                throw new InvalidOperationException($"{first.Wallet} doesn't have enough money.");

            first.Wallet.Spend(finalCost);
            second.Wallet.Add(finalCost);

            first.Inventory.Add(item);
            second.Inventory.Remove(item);
        }
    }
}
