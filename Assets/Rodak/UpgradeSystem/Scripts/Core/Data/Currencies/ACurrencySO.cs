using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class ACurrencySO : UniqueScriptableObject<ACurrencySO>
    {
        [SerializeField] private string saveId;
        [SerializeField] private string currencyName;
        [SerializeField] private Color currencyColor = Color.white;
        [SerializeField] private BigDouble baseValue;

        public string SaveId => saveId;
        public string CurrencyName => currencyName;

        public Color CurrencyColor => currencyColor;
        public BigDouble BaseValue => baseValue;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(saveId))
            {
                Debug.LogWarning($"Missing saveId for '{name}'");
            }
        }
#endif
        public override string ToString()
        {
            return CurrencyPresenter.GetFormattedCurrency(this);
        }
    }
}