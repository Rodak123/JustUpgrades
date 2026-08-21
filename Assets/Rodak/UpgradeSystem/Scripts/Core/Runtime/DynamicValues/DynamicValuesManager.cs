using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class DynamicValuesManager : MonoBehaviour
    {
        [SerializeField] private List<ADynamicValueReferenceSO> dynamicValueReferences = new();

        private readonly Dictionary<ADynamicValueReferenceSO, List<IDynamicValueModifier>> valueModifiersMap = new();

        public void Init()
        {
            foreach (ADynamicValueReferenceSO valueReference in dynamicValueReferences)
            {
                valueModifiersMap.Add(valueReference, new());
            }
        }

        private void Query(DynamicValueQuery query, ADynamicValueReferenceSO valueReference)
        {
            if (!valueModifiersMap.TryGetValue(valueReference, out List<IDynamicValueModifier> valueModifiers))
            {
                Debug.LogError($"Value '{valueReference}' is not managed.");
                return;
            }

            foreach (IDynamicValueModifier modifier in valueModifiers)
            {
                modifier.Query(query);
            }
        }

        public void AttachModifier(ADynamicValueReferenceSO valueReference, IDynamicValueModifier valueModifier)
        {
            if (!valueModifiersMap.TryGetValue(valueReference, out List<IDynamicValueModifier> valueModifiers))
            {
                Debug.LogError($"Value '{valueReference}' is not managed.");
                return;
            }

            valueModifiers.Add(valueModifier);
        }

        public void RemoveModifier(ADynamicValueReferenceSO valueReference, IDynamicValueModifier valueModifier)
        {
            if (!valueModifiersMap.TryGetValue(valueReference, out List<IDynamicValueModifier> valueModifiers))
            {
                Debug.LogError($"Value '{valueReference}' is not managed.");
                return;
            }

            valueModifiers.Remove(valueModifier);
        }

        public BigDouble QueryFloat(FloatDynamicValue floatDynamicValue)
        {
            DynamicValueQuery query = new()
            {
                Value = floatDynamicValue.GetBaseAmount(),
            };

            if (floatDynamicValue.IsExposed)
            {
                Query(query, floatDynamicValue.ExposedReference);
            }

            return query.Value;
        }
    }
}