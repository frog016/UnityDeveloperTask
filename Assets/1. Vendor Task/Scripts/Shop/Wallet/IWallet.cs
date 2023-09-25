using System;

namespace VendorTask.Shop.Wallet
{
    public interface IWallet
    {
        int Balance { get; }
        event Action<int> BalanceUpdated;
        void Add(int amount);
        void Spend(int amount);
        bool IsEnough(int amount);
    }
}