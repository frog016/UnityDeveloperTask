using VendorTask.UI;

namespace VendorTask.Shop.Operation
{
    public abstract class ShopOperation : IOperation
    {
        protected ItemSlot Slot { get; private set; }
        protected ItemView Content { get; private set; }

        private InventoryUI _previousContentOwner;

        public void SetContext(ItemSlot slot, ItemView content, InventoryUI previousContentOwner)
        {
            Slot = slot;
            Content = content;
            _previousContentOwner = previousContentOwner;
        }

        public abstract void Accept();
        public abstract void Undo();
        public abstract bool IsValid();

        protected void RemoveContentFromPrevious()
        {
            _previousContentOwner.RemoveContentFromSlot(Content);
        }
    }
}