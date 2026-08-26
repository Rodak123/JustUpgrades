using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class IntDynamicValue : ADynamicValue<IntDynamicValueReferenceSO>
    {
        [SerializeField] private int baseValue;

        private int? minValue;
        private int? maxValue;

        public IntDynamicValue() : this(0)
        { }

        public IntDynamicValue(int baseValue, int? minValue = null, int? maxValue = null)
        {
            this.baseValue = baseValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        private int Clamp(int value)
        {
            if (minValue.HasValue && value < minValue.Value) value = minValue.Value;
            if (maxValue.HasValue && value > maxValue.Value) value = maxValue.Value;
            return value;
        }

        public override BigDouble GetBaseValue()
        {
            return Clamp(baseValue);
        }

        public int GetIntValue(DynamicValuesModifiers dynamicValuesModifiers)
        {
            int value = GetValue(dynamicValuesModifiers).FloorToInt();
            value = Clamp(value);
            return value;
        }
    }
}