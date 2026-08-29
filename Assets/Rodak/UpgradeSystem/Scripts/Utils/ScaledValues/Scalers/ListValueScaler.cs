using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ListValueScaler : IValueScaler, IHasDynamicValues
    {
        [SerializeField] private List<FloatDynamicValue> values;
        [Tooltip("When true, the sum of all previous values is added")]
        [SerializeField] private BoolDynamicValue additive;
        [SerializeField] private IntDynamicValue ignoredBeforeLevel = new(1, 0);

        public BigDouble GetValue(int level, int maxLevel)
        {
            if (values.Count == 0) return 0;

            int ignoredBeforeLevelValue = ignoredBeforeLevel.GetIntValue();
            if (level < ignoredBeforeLevelValue) return 0;

            int index = Math.Clamp(level - ignoredBeforeLevelValue, 0, values.Count - 1);

            bool additiveValue = additive.GetBoolValue();

            if (!additiveValue)
            {
                BigDouble value = values[index].GetFloatValue();
                return value;
            }

            BigDouble sum = 0;
            for (int i = 0; i <= index; i++)
            {
                BigDouble value = values[i].GetFloatValue();
                sum += value;
            }
            return sum;
        }

        public string Hint() => "x = list[level - ignoredBeforeLevel]";

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            foreach (FloatDynamicValue value in values)
            {
                yield return value;
            }
            yield return ignoredBeforeLevel;
        }
    }
}