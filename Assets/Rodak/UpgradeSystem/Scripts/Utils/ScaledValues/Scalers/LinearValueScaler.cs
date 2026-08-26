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
            BigDouble baseValueValue = baseValue.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);
            BigDouble incrementValue = increment.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);
            BigDouble incrementGrowValue = incrementGrow.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);

            return baseValueValue + (incrementValue + (incrementGrowValue * level)) * level;
        }

        public string Hint() => "x = base + (increment + (grow * level)) * level";

        public IEnumerable<ADynamicValueReferenceSO> GetDynamicValueReferences()
        {
            yield return baseValue.ExposedReference;
            yield return increment.ExposedReference;
            yield return incrementGrow.ExposedReference;
        }
    }
}