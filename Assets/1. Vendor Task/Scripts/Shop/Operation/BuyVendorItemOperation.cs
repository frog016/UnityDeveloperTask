using UnityEngine;

namespace VendorTask.Shop.Operation
{
    public class BuyVendorItemOperation : ShopOperation
    {
        private readonly VendorShop _shop;

        public BuyVendorItemOperation(VendorShop shop)
        {
            _shop = shop;
        }

        public override void Accept()
        {
            RemoveContentFromPrevious();
            Slot.SetContent(Content);
            _shop.Sell(Content.Item);
        }

        public override void Undo()
        {
            Content.UndoDrag();
            Debug.Log("You don't have enough money in your wallet.");
        }

        public override bool IsValid()
        {
            var cost = Content.Item.Cost;
            return _shop.CustomerPerson.Wallet.IsEnough(cost);
        }
    }
}