using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class IntDynamicValue : ADynamicValue<IntDynamicValueReferenceSO>
    {
        [SerializeField] private BigDouble baseValue;

        public IntDynamicValue() : this(0)
        { }

        public IntDynamicValue(BigDouble baseValue)
        {
            this.baseValue = baseValue;
        }

        public override BigDouble GetBaseValue()
        {
            return baseValue.Round();
        }

        public BigDouble GetIntValue(DynamicValuesModifiers dynamicValuesModifiers)
        {
            return GetValue(dynamicValuesModifiers).Round();
        }
    }
}