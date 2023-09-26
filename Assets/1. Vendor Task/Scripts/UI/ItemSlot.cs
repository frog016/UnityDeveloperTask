using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VendorTask.UI
{
    public class ItemSlot : UIElement, IDropHandler
    {
        public UIElement Content { get; private set; }

        public void Initialize(UIElement content)
        {
            Content = content;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (Content != null || eventData.pointerDrag.TryGetComponent<DragAndDropElement>(out var element) == false)
                return;

            Content = element;
            element.SetEventData(new DragAndDropEventData(
                RectTransform.position,
                RemoveContent));
        }

        private void RemoveContent()
        {
            Content = null;
        }

        #region Debug

        protected override void OnAwake()
        {
            _debugColor = GetComponent<Image>().color;
        }

        private Color _debugColor;

        private void OnDrawGizmos()
        {
            var color = Content == null ? _debugColor : Color.red;
            GetComponent<Image>().color = color;
        }

        #endregion
    }
}