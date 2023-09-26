using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VendorTask.UI
{
    public class ItemSlot : UIElement, IDropHandler
    {
        public ItemView Content { get; private set; }
        public bool IsEmpty => Content == null;
        public event Action<ItemView> ItemDropped;

        public void Initialize(ItemView content)
        {
            Content = content;
            Content.RectTransform.position = RectTransform.position;
        }

        public void RemoveContent()
        {
            Content = null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (Content != null || eventData.pointerDrag.TryGetComponent<ItemView>(out var element) == false)
                return;

            Content = element;
            element.SetEventData(new DragAndDropEventData(
                RectTransform.position,
                RemoveContent));
            ItemDropped?.Invoke(Content);
        }

        #region Debug

        protected override void OnAwake()
        {
            _debugColor = GetComponent<Image>().color;
        }

        private Color _debugColor;

        private void OnDrawGizmos()
        {
            //var color = Content == null ? _debugColor : Color.red;
            //GetComponent<Image>().color = color;
        }

        #endregion
    }
}