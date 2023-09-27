namespace VendorTask.Shop.Operation
{
    public class ReplaceItemOperation : ShopOperation
    {
        public override void Accept()
        {
            Slot.Initialize(Content);
        }

        public override void Undo()
        {
            Content.UndoDrag();
        }

        public override bool IsValid()
        {
            return Slot.IsEmpty;
        }
    }
}