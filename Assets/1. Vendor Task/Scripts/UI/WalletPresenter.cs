using System;
using VendorTask.Shop.Wallet;

namespace VendorTask.UI
{
    public class WalletPresenter : IDisposable
    {
        private readonly IWallet _wallet;
        private readonly WalletView _walletView;

        public WalletPresenter(IWallet wallet, WalletView walletView)
        {
            _wallet = wallet;
            _walletView = walletView;
        }

        public void Initialize()
        {
            OnBalanceChanged(_wallet.Balance);
            _wallet.BalanceUpdated += OnBalanceChanged;
        }

        public void Dispose()
        {
            _wallet.BalanceUpdated -= OnBalanceChanged;
        }

        private void OnBalanceChanged(int newValue)
        {
            _walletView.SetBalanceValue(newValue);
        }
    }
}