using System;
using UnityEngine;

namespace VendorTask.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIElement : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }

        private Canvas _parentCanvas;

        protected void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();

            if (_parentCanvas == null)
                throw new Exception("UIElement must have Canvas in its parent.");

            OnAwake();
        }

        public void TranslateScaledAnchoredPosition(Vector2 delta)
        {
            RectTransform.anchoredPosition += delta / _parentCanvas.scaleFactor;
        }

        protected virtual void OnAwake() { }
    }
}