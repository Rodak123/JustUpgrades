using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class FloatDynamicValue : ADynamicValue<FloatDynamicValueReferenceSO>
    {
        public static BigDouble GetFloatValueFrom(BigDouble value)
        {
            return value;
        }

        [SerializeField] private BigDouble baseValue;

        public FloatDynamicValue() : this(0)
        { }

        public FloatDynamicValue(BigDouble baseValue, BigDouble? minValue = null, BigDouble? maxValue = null) : base(minValue, maxValue)
        {
            this.baseValue = baseValue;
        }

        protected override BigDouble GetBaseValue()
        {
            return baseValue;
        }

        public BigDouble GetFloatValue() => GetFloatValueFrom(GetValue());
    }
}