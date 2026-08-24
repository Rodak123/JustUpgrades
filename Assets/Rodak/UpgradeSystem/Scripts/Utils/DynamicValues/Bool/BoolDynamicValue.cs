using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class BoolDynamicValue : ADynamicValue<BoolDynamicValueReferenceSO>
    {
        [SerializeField] private bool baseValue;

        public BoolDynamicValue() : this(false)
        { }

        public BoolDynamicValue(bool baseValue)
        {
            this.baseValue = baseValue;
        }

        public override BigDouble GetBaseValue()
        {
            return baseValue ? 1 : 0;
        }

        public bool GetBoolValue(DynamicValuesModifiers dynamicValuesModifiers)
        {
            return GetValue(dynamicValuesModifiers) > 0;
        }
    }
}