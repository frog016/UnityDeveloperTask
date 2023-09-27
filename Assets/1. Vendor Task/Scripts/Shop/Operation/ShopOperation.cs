using VendorTask.UI;

namespace VendorTask.Shop.Operation
{
    public abstract class ShopOperation : IOperation
    {
        protected ItemSlot Slot { get; private set; }
        protected ItemView Content { get; private set; }

        public void SetContext(ItemSlot slot, ItemView content)
        {
            Slot = slot;
            Content = content;
        }

        public abstract void Accept();
        public abstract void Undo();
        public abstract bool IsValid();
    }
}