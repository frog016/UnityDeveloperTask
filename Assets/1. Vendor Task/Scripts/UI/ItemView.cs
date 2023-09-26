using System;
using TMPro;
using UnityEngine;
using VendorTask.Items;

namespace VendorTask.UI
{
    public class ItemView : DragAndDropElement
    {
        [SerializeField] private TextMeshProUGUI _nameTextView;
        [SerializeField] private TextMeshProUGUI _costTextView;

        public Item Item { get; private set; }

        public void Initialize(Item item)
        {
            Item = item;

            _nameTextView.text = item.ToString();
            _costTextView.text = item.Cost.ToString();
        }
    }
}