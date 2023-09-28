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
        public event Action<ItemSlot, ItemView> ItemDropped;

        public void SetContent(ItemView content)
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

            element.AcceptDrag(RectTransform.position);
            ItemDropped?.Invoke(this, element);
        }

        #region Debug

        [SerializeField] private Color _debugColor;

        private void OnDrawGizmos()
        {
            var color = Content == null ? _debugColor : Color.red;
            GetComponent<Image>().color = color;
        }

        #endregion
    }
}