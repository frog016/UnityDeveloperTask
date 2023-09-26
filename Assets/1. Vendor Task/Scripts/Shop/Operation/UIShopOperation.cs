using VendorTask.UI;

namespace VendorTask.Shop.Operation
{
    public abstract class UIShopOperation : IOperation
    {
        protected readonly ItemView ItemView;

        protected UIShopOperation(ItemView itemView)
        {
            ItemView = itemView;
        }

        public abstract void Accept();
        public abstract void Undo();
        public abstract bool IsValid();
    }
}