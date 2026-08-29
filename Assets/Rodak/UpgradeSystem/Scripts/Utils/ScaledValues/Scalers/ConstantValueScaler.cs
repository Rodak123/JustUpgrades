using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ConstantValueScaler : IValueScaler, IHasDynamicValues
    {

        [SerializeField] private FloatDynamicValue value = new(1);
        [SerializeField] private IntDynamicValue ignoredBeforeLevel = new(0, 0);

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble valueValue = value.GetFloatValue();
            int ignoredBeforeLevelValue = ignoredBeforeLevel.GetIntValue();

            if (level < ignoredBeforeLevelValue) return 0;
            return valueValue;
        }

        public string Hint() => "x = value";

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return value;
            yield return ignoredBeforeLevel;
        }
    }
}