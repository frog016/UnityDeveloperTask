using System;
using System.Collections.Generic;
using System.Linq;
using VendorTask.Shop.Operation;
using VendorTask.UI;
using Object = UnityEngine.Object;

namespace VendorTask
{
    public class InventoryInputController : IDisposable
    {
        private readonly InventoryUI _playerUI;
        private readonly InventoryUI _vendorUI;

        private readonly Dictionary<Type, IOperation> _operations;

        public InventoryInputController(InventoryUI playerUI, InventoryUI vendorUI, IEnumerable<IOperation> operations)
        {
            _playerUI = playerUI;
            _vendorUI = vendorUI;

            _operations = operations.ToDictionary(key => key.GetType(), value => value);
        }

        public void Initialize()
        {
            _playerUI.ItemDroppedInSlot += OnItemDroppedInput;
        }

        public void Dispose()
        {
            _playerUI.ItemDroppedInSlot -= OnItemDroppedInput;
        }

        private void OnItemDroppedInput(ItemSlot slot, ItemView view)
        {
            var slotOwnerInventoryUI = _playerUI.Contains(slot) ? _playerUI : _vendorUI;
            var contentOwnerInventoryUI = _playerUI.ContainsContent(view) ? _playerUI : _vendorUI;

            var operation = GetOperationByInput(slotOwnerInventoryUI, contentOwnerInventoryUI);
            operation.SetContext(slot, view);

            if (operation.IsValid())
                operation.Accept();
            else
                operation.Undo();
        }

        private ShopOperation GetOperationByInput(Object slotOwner, Object contentOwner)
        {
            if (slotOwner == contentOwner)
                return (ShopOperation)_operations[typeof(ReplaceItemOperation)];

            if (contentOwner == _playerUI && slotOwner == _vendorUI)
                return (ShopOperation)_operations[typeof(SellItemToVendorOperation)];

            if (contentOwner == _vendorUI && slotOwner == _playerUI)
                return (ShopOperation)_operations[typeof(BuyVendorItemOperation)];

            throw new InvalidOperationException("Unknown operation");
        }
    }
}