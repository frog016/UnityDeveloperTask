using System;
using UnityEngine;

namespace VendorTask.UI
{
    public readonly struct DragAndDropEventData
    {
        public readonly Vector2 DropPosition;
        public readonly Action OnStartDragCallback;

        public DragAndDropEventData(Vector2 dropPosition, Action onStartDragCallback = null)
        {
            DropPosition = dropPosition;
            OnStartDragCallback = onStartDragCallback;
        }

        public DragAndDropEventData Copy()
        {
            return new DragAndDropEventData(DropPosition);
        }
    }
}