using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    public class DynamicValuesModifiers : MonoBehaviour
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

        public void AddDynamicValueReference(ADynamicValueReferenceSO dynamicValueReferenceSO)
        {
            if (dynamicValueReferenceSO == null || valueModifiersMap.ContainsKey(dynamicValueReferenceSO))
            {
                return;
            }
            valueModifiersMap.Add(dynamicValueReferenceSO, new());
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

        public void QueryModifiersFor(DynamicValueQuery query, ADynamicValueReferenceSO valueReference)
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

    }
}