using System;

namespace VendorTask.Shop.Wallet
{
    public class InfiniteWallet : IWallet
    {
        public int Balance => int.MaxValue;
        public event Action<int> BalanceUpdated;

        public void Add(int amount) { }

        public void Spend(int amount) { }

        public bool IsEnough(int amount) => true;
    }
}