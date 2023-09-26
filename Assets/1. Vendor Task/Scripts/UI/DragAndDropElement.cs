using UnityEngine;
using UnityEngine.EventSystems;

namespace VendorTask.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DragAndDropElement : UIElement, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private CanvasGroup _canvasGroup;
        private DragAndDropEventData _eventData;

        protected override void OnAwake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            _eventData.OnStartDragCallback?.Invoke();
            _eventData = new DragAndDropEventData(RectTransform.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            TranslateScaledAnchoredPosition(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            RectTransform.position = _eventData.DropPosition;
        }

        public void SetEventData(DragAndDropEventData eventData)
        {
            _eventData = eventData;
        }
    }
}