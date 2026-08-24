using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class FloatDynamicValue : ADynamicValue<FloatDynamicValueReferenceSO>
    {
        [SerializeField] private BigDouble baseValue;

        public FloatDynamicValue() : this(0)
        { }

        public FloatDynamicValue(BigDouble baseValue)
        {
            this.baseValue = baseValue;
        }

        public override BigDouble GetBaseValue()
        {
            return baseValue;
        }

        public BigDouble GetFloatValue(DynamicValuesModifiers dynamicValuesModifiers)
        {
            return GetValue(dynamicValuesModifiers);
        }
    }
}