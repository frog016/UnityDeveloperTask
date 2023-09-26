using System;
using UnityEditor;
using UnityEngine;

namespace VendorTask.Items
{
    [CreateAssetMenu(menuName = "Configs/Item", fileName = "ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField, Min(0)] private int _cost;
        [SerializeField] private string _name;

        public int Id => _id;

        [ContextMenu(nameof(ChangeAssetName))]
        private void ChangeAssetName()
        {
            var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, _name);
            AssetDatabase.SaveAssets();
        }

        public Item CreateInstance()
        {
            return new Item(_id, _cost, _name);
        }
    }
}