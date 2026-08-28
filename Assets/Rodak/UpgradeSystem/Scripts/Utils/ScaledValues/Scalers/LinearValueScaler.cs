using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class LinearValueScaler : IValueScaler, IHasDynamicValues
    {
        [SerializeField] private FloatDynamicValue baseValue = new(0);
        [SerializeField] private FloatDynamicValue increment = new(1);
        [SerializeField] private FloatDynamicValue incrementGrow = new(0);

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble baseValueValue = baseValue.GetFloatValue();
            BigDouble incrementValue = increment.GetFloatValue();
            BigDouble incrementGrowValue = incrementGrow.GetFloatValue();

            return baseValueValue + (incrementValue + (incrementGrowValue * level)) * level;
        }

        public string Hint() => "x = base + (increment + (grow * level)) * level";

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return baseValue;
            yield return increment;
            yield return incrementGrow;
        }
    }
}