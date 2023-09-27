using UnityEngine;
using UnityEngine.EventSystems;

namespace VendorTask.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DragAndDropElement : UIElement, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private CanvasGroup _canvasGroup;
        private Vector2 _oldPosition;
        private bool _isDragSuccessful;

        protected override void OnAwake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            _oldPosition = RectTransform.position;
            _isDragSuccessful = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            TranslateScaledAnchoredPosition(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            if (_isDragSuccessful == false)
                UndoDrag();
        }

        public void AcceptDrag(Vector2 position)
        {
            RectTransform.position = position;
            _isDragSuccessful = true;
        }

        public void UndoDrag()
        {
            RectTransform.position = _oldPosition;
        }
    }
}