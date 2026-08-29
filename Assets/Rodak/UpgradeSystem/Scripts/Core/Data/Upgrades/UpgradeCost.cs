using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeCost : IHasDynamicValues
    {
        [SerializeField] private ACurrencySO targetCurrency;
        [SerializeField] private ScaledValueCollection costScaling;
        [SerializeField] private BoolDynamicValue isSpent = new(true);

        public ACurrencySO TargetCurrency => targetCurrency;
        public BoolDynamicValue IsSpent => isSpent;

        public BigDouble GetCostValue(int currentLevel, int maxLevel) => new(costScaling.GetValue(currentLevel, maxLevel));

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return isSpent;
            foreach (IDynamicValue dynamicValue in costScaling.GetDynamicValues())
            {
                yield return dynamicValue;
            }
        }
    }
}