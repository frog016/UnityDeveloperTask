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
            Slot.Initialize(Content);
            _shop.Sell(Content.Item);
        }

        public override void Undo()
        {
            Content.UndoDrag();
            //  TODO: Send notification about lack of money.
        }

        public override bool IsValid()
        {
            var cost = Content.Item.Cost;
            return _shop.CustomerPerson.Wallet.IsEnough(cost);
        }
    }
}