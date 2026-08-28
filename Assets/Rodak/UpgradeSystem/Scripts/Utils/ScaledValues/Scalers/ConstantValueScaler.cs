using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ConstantValueScaler : IValueScaler, IHasDynamicValues
    {

        [SerializeField] private FloatDynamicValue value = new(1);

        [Tooltip("When true, the level 0 value is 0")]
        [SerializeField] private BoolDynamicValue ignoredAtZero = new(false);

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble valueValue = value.GetFloatValue();
            bool ignoredAtZeroValue = ignoredAtZero.GetBoolValue();

            if (ignoredAtZeroValue && level == 0) return 0;
            return valueValue;
        }

        public string Hint() => "x = value";

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return value;
            yield return ignoredAtZero;
        }
    }
}