namespace VendorTask.Shop.Operation
{
    public class SellItemToVendorOperation : ShopOperation
    {
        private readonly VendorShop _shop;

        public SellItemToVendorOperation(VendorShop shop)
        {
            _shop = shop;
        }

        public override void Accept()
        {
            Slot.Initialize(Content);
            _shop.Buy(Content.Item);
        }

        public override void Undo()
        {
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}