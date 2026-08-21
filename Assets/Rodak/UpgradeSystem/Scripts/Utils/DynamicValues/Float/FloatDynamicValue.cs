using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class FloatDynamicValue : ADynamicValue
    {
        [SerializeField] private BigDouble baseAmount;
        [SerializeField] private FloatDynamicValueReferenceSO exposedReference;

        public bool IsExposed => exposedReference != null;

        public FloatDynamicValueReferenceSO ExposedReference => exposedReference;

        public void RemoveReference()
        {
            if (exposedReference == null) return;
            exposedReference = null;
        }

        public FloatDynamicValueReferenceSO AddReference()
        {
            if (exposedReference != null) return null;

            FloatDynamicValueReferenceSO floatValueChanger = ScriptableObject.CreateInstance<FloatDynamicValueReferenceSO>();

            exposedReference = floatValueChanger;

            return floatValueChanger;
        }

        public override BigDouble GetBaseAmount()
        {
            return baseAmount;
        }
    }
}