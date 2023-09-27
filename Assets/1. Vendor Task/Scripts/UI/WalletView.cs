using TMPro;
using UnityEngine;

namespace VendorTask.UI
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _balanceTextView;

        public void SetBalanceValue(int value)
        {
            _balanceTextView.text = value.ToString();
        }
    }
}