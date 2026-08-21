using System;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class LinearValueScaler : IValueScaler
    {
        [SerializeReference, MarkDynamicValue(ValueName = "BaseValue")] private FloatDynamicValue baseValue = new(0);
        [SerializeReference, MarkDynamicValue(ValueName = "Increment")] private FloatDynamicValue increment = new(1);
        [SerializeReference, MarkDynamicValue(ValueName = "IncrementGrow")] private FloatDynamicValue incrementGrow = new(0);

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble baseValueValue = UpgradeSystemContext.Instance.DynamicValuesManager.QueryFloat(baseValue);
            BigDouble incrementValue = UpgradeSystemContext.Instance.DynamicValuesManager.QueryFloat(increment);
            BigDouble incrementGrowValue = UpgradeSystemContext.Instance.DynamicValuesManager.QueryFloat(incrementGrow);

            return baseValueValue + (incrementValue + (incrementGrowValue * level)) * level;
        }

        public string Hint() => "x = base + (increment + (grow * level)) * level";
    }
}