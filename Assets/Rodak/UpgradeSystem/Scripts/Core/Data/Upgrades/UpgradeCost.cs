using System;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeCost
    {
        [SerializeField] private CurrencySO targetCurrency;
        [SerializeField] private ScaledValueCollection costScaling;

        public CurrencySO TargetCurrency => targetCurrency;

        public BigDouble GetCostValue(int currentLevel, int maxLevel) => new(costScaling.GetValue(currentLevel, maxLevel));
    }
}