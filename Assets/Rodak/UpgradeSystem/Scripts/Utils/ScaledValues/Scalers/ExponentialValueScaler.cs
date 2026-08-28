using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ExponentialValueScaler : IValueScaler, IHasDynamicValues
    {
        [SerializeField] private FloatDynamicValue baseValue = new(1);
        [SerializeField] private FloatDynamicValue multiplier = new(1.15);
        [SerializeField] private FloatDynamicValue multiplierGrow = new(0);

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble baseValueValue = baseValue.GetFloatValue();
            BigDouble multiplierValue = multiplier.GetFloatValue();
            BigDouble multiplierGrowValue = multiplierGrow.GetFloatValue();

            return baseValueValue * BigDouble.Pow(multiplierValue + multiplierGrowValue * level, level);
        }

        public string Hint() => "x = base * (multiplier + (grow * level)) ^ level";

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return baseValue;
            yield return multiplier;
            yield return multiplierGrow;
        }
    }
}