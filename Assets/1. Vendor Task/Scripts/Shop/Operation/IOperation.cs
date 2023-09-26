namespace VendorTask.Shop.Operation
{
    public interface IOperation
    {
        void Accept();
        void Undo();
        bool IsValid();
    }
}
