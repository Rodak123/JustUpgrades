using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class IntDynamicValue : ADynamicValue<IntDynamicValueReferenceSO>
    {
        public static int GetIntValueFrom(BigDouble value)
        {
            return value.FloorToInt();
        }

        [SerializeField] private int baseValue;

        public IntDynamicValue() : this(0)
        { }

        public IntDynamicValue(int baseValue, int? minValue = null, int? maxValue = null) : base(minValue, maxValue)
        {
            this.baseValue = baseValue;
        }

        protected override BigDouble GetBaseValue()
        {
            return baseValue;
        }

        public int GetIntValue() => GetIntValueFrom(GetValue());
    }
}