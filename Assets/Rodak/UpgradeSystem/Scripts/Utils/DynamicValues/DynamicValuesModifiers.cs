using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    public class DynamicValuesModifiers : MonoBehaviour
    {
        [Serializable]
        private struct DynamicValueReferenceGroup
        {
            public string groupName;
            public List<ADynamicValueReferenceSO> dynamicValueReferences;
        }

        [Header("Note: Dynamic values under registered upgrades are registered automatically")]
        [Header("Grouping only to organize")]
        [SerializeField] private List<DynamicValueReferenceGroup> dynamicValueReferenceGroups = new();

        private readonly Dictionary<ADynamicValueReferenceSO, List<IDynamicValueModifier>> valueModifiersMap = new();

        public void Init()
        {
            for (int i = 0; i < dynamicValueReferenceGroups.Count; i++)
            {
                List<ADynamicValueReferenceSO> groupUpgrades = dynamicValueReferenceGroups[i].dynamicValueReferences;
                for (int j = 0; j < groupUpgrades.Count; j++)
                {
                    ADynamicValueReferenceSO valueReference = groupUpgrades[j];
                    if (valueReference == null)
                    {
                        Debug.LogWarning($"Null value reference at [{j}] in group: '{dynamicValueReferenceGroups[i].groupName}'");
                        continue;
                    }
                    valueModifiersMap.Add(valueReference, new());
                }
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