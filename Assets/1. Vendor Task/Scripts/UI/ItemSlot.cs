using UnityEngine;

namespace VendorTask.UI
{
    public class ItemSlot : MonoBehaviour
    {
        private Transform _content;

        public void SetContent(Transform content)
        {
            _content = content;
            _content.SetParent(transform);
        }

        public void RemoveContent()
        {
            if (_content == null)
                return;

            _content.SetParent(null);
        }
    }
}