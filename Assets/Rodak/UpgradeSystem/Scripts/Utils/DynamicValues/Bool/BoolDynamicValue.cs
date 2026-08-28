using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class BoolDynamicValue : ADynamicValue<BoolDynamicValueReferenceSO>
    {
        public static bool GetBoolValueFrom(BigDouble value)
        {
            return value > 0;
        }

        [SerializeField] private bool baseValue;

        public BoolDynamicValue() : this(false)
        { }

        public BoolDynamicValue(bool baseValue) : base(0, 1)
        {
            this.baseValue = baseValue;
        }

        protected override BigDouble GetBaseValue()
        {
            return baseValue ? 1 : 0;
        }

        public bool GetBoolValue() => GetBoolValueFrom(GetValue());
    }
}