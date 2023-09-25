using System;

namespace VendorTask.Shop.Wallet
{
    public class CommonWallet : IWallet
    {
        public int Balance { get; private set; }
        public event Action<int> BalanceUpdated;

        public CommonWallet(int balance)
        {
            Balance = balance;
        }

        public void Add(int amount)
        {
            AssertIfNegative(amount);

            Balance += amount;
            BalanceUpdated?.Invoke(Balance);
        }

        public void Spend(int amount)
        {
            AssertIfNegative(amount);
            if (IsEnough(amount) == false)
                throw new InvalidOperationException(nameof(amount));

            Balance -= amount;
            BalanceUpdated?.Invoke(Balance);
        }

        public bool IsEnough(int amount)
        {
            return Balance >= amount;
        }

        private static void AssertIfNegative(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
        }
    }
}