using System;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public abstract class ADynamicValue<R> : IDynamicValue where R : ADynamicValueReferenceSO
    {
        private class BaseModifier : IDynamicValueModifier
        {
            private ADynamicValue<R> dynamicValue;

            public BaseModifier(ADynamicValue<R> dynamicValue)
            {
                this.dynamicValue = dynamicValue;
            }

            public void Query(DynamicValueQuery query)
            {
                query.Value += dynamicValue.GetBaseValue();
            }
        }

        [SerializeField] private R exposedReference;

        public bool IsExposed => exposedReference != null;
        public R ExposedReferenceTyped => exposedReference;
        public ADynamicValueReferenceSO ExposedReference => exposedReference;

        private BigDouble? minValue;
        private BigDouble? maxValue;

        public ADynamicValue(BigDouble? minValue, BigDouble? maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        protected abstract BigDouble GetBaseValue();

        public BigDouble ProcessValue(BigDouble value)
        {
            if (minValue.HasValue && value < minValue.Value) value = minValue.Value;
            if (maxValue.HasValue && value > maxValue.Value) value = maxValue.Value;
            return value;
        }

        public IDynamicValueModifier GetBaseModifier()
        {
            return new BaseModifier(this);
        }

        public BigDouble GetValue()
        {
            if (IsExposed) return exposedReference.GetValue();
            return GetBaseValue();
        }
    }
}