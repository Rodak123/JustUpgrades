using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeCosts : IHasDynamicValues
    {
        [SerializeField] private List<UpgradeCost> costs;

        public IReadOnlyList<UpgradeCost> Costs => costs;

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            foreach (UpgradeCost cost in costs)
            {
                foreach (IDynamicValue dynamicValue in cost.GetDynamicValues())
                {
                    yield return dynamicValue;
                }
            }
        }

        public bool CanAffordAt(int currentLevel, int maxLevel)
        {
            foreach (UpgradeCost cost in Costs)
            {
                ACurrencySO currency = cost.TargetCurrency;

                if (currency == null) continue;
                BigDouble costValue = cost.GetCostValue(currentLevel, maxLevel);

                if (!UpgradeSystemContext.Instance.CurrencyWallet.CanSpend(currency, costValue))
                    return false;
            }
            return true;
        }
    }
}