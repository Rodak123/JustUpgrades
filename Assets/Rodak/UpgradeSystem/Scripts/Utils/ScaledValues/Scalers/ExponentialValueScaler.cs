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
            BigDouble baseValueValue = baseValue.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);
            BigDouble multiplierValue = multiplier.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);
            BigDouble multiplierGrowValue = multiplierGrow.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);

            return baseValueValue * BigDouble.Pow(multiplierValue + multiplierGrowValue * level, level);
        }

        public string Hint() => "x = base * (multiplier + (grow * level)) ^ level";

        public IEnumerable<ADynamicValueReferenceSO> GetDynamicValueReferences()
        {
            yield return baseValue.ExposedReference;
            yield return multiplier.ExposedReference;
            yield return multiplierGrow.ExposedReference;
        }
    }
}