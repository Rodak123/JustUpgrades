using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Core/Currency", order = 100)]
    public class CurrencySO : UniqueScriptableObject<CurrencySO>
    {
        [SerializeField] private string currencyName;
        [SerializeField] private Color currencyColor = Color.white;
        [SerializeField] private BigDouble baseValue;

        public string CurrencyName => currencyName;
        public Color CurrencyColor => currencyColor;
        public BigDouble BaseValue => baseValue;

        private void Reset()
        {
            currencyName = name;
        }

        public override string ToString()
        {
            return CurrencyPresenter.GetFormattedCurrency(this);
        }
    }
}