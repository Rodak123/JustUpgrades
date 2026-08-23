using System;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class LinearValueScaler : IValueScaler
    {
        [SerializeField] private FloatDynamicValue baseValue = new(0);
        [SerializeField] private FloatDynamicValue increment = new(1);
        [SerializeField] private FloatDynamicValue incrementGrow = new(0);

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