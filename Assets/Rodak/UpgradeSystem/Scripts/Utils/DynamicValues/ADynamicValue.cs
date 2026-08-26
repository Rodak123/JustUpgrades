using System;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public abstract class ADynamicValue<R> where R : ADynamicValueReferenceSO
    {
        [SerializeField] private R exposedReference;

        public bool IsExposed => exposedReference != null;
        public R ExposedReference => exposedReference;

        public ADynamicValue()
        { }

        public abstract BigDouble GetBaseValue();

        public BigDouble GetValue(DynamicValuesModifiers dynamicValuesModifiers)
        {
            BigDouble baseValue = GetBaseValue();

            DynamicValueQuery query = new()
            {
                Value = baseValue,
            };

            if (IsExposed)
            {
                dynamicValuesModifiers.QueryModifiersFor(query, ExposedReference);
            }

            return query.Value;
        }
    }
}