using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Core/Currency Group", order = 100)]
    public class CurrencyGroupSO : ACurrencyContainerSO
    {
        [SerializeField] private List<ACurrencySO> currencies;

        public override IEnumerable<ACurrencySO> GetCurrencySOs()
        {
            return currencies;
        }
        public override string ToString()
        {
            return $"CurrencyGroup[{currencies.Count}]";
        }
    }
}