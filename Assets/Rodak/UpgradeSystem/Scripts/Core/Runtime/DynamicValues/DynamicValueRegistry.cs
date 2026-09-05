using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rodak.UpgradeSystem.Core
{
    public class DynamicValueRegistry : MonoBehaviour
    {
        [Serializable]
        private struct DynamicValueContainerGroup
        {
            public string groupName;
            public List<Object> dynamicValueContainers;
        }

        [Header("Note: Dynamic values under registered upgrades are registered automatically")]
        [Header("Grouping only to organize")]
        [SerializeField] private List<DynamicValueContainerGroup> dynamicValueContainerGroups = new();

        private readonly Dictionary<ADynamicValueReferenceSO, DynamicValueHandler> dynamicValueHandlers = new();

        public void Init()
        {
            for (int i = 0; i < dynamicValueContainerGroups.Count; i++)
            {
                List<Object> groupContainers = dynamicValueContainerGroups[i].dynamicValueContainers;
                for (int j = 0; j < groupContainers.Count; j++)
                {
                    Object container = groupContainers[j];

                    if (container is GameObject gameObject)
                    {
                        RegisterDynamicValuesFrom(gameObject);
                        continue;
                    }

                    if (container is IHasDynamicValues hasDynamicValues)
                    {
                        RegisterDynamicValuesFrom(hasDynamicValues);
                        continue;
                    }

                    Debug.LogWarning($"Container '{container}' at [{j}] in group '{dynamicValueContainerGroups[i].groupName}' does not implement ${nameof(IHasDynamicValues)}, ignoring.");
                }
            }
        }

        public bool RegisterDynamicValuesFrom(GameObject gameObject)
        {
            IHasDynamicValues[] hasDynamicValuesList = gameObject.GetComponents<IHasDynamicValues>();
            if (hasDynamicValuesList.Length == 0)
            {
                Debug.LogWarning($"{gameObject} does have any components which implement ${nameof(IHasDynamicValues)}, ignoring.");
                return false;
            }

            foreach (IHasDynamicValues hasDynamicValues in hasDynamicValuesList)
            {
                RegisterDynamicValuesFrom(hasDynamicValues);
            }

            return true;
        }

        public void RegisterDynamicValuesFrom(IHasDynamicValues hasDynamicValues)
        {
            foreach (IDynamicValue dynamicValue in hasDynamicValues.GetDynamicValues())
            {
                RegisterDynamicValue(dynamicValue);
            }
        }

        public bool RegisterDynamicValue(IDynamicValue dynamicValue)
        {
            if (!dynamicValue.IsExposed || dynamicValueHandlers.ContainsKey(dynamicValue.ExposedReference))
            {
                return false;
            }
            dynamicValueHandlers.Add(dynamicValue.ExposedReference, new(dynamicValue));
            return true;
        }

        public void AttachModifier(ADynamicValueReferenceSO valueReference, IDynamicValueModifier valueModifier)
        {
            if (!dynamicValueHandlers.TryGetValue(valueReference, out DynamicValueHandler handler))
            {
                Debug.LogError($"Value '{valueReference}' is not registered.");
                return;
            }

            handler.AttachModifier(valueModifier);
        }

        public void RemoveModifier(ADynamicValueReferenceSO valueReference, IDynamicValueModifier valueModifier)
        {
            if (!dynamicValueHandlers.TryGetValue(valueReference, out DynamicValueHandler handler))
            {
                Debug.LogError($"Value '{valueReference}' is not registered.");
                return;
            }

            handler.RemoveModifier(valueModifier);
        }

        public BigDouble QueryValue(ADynamicValueReferenceSO valueReference)
        {
            if (!dynamicValueHandlers.TryGetValue(valueReference, out DynamicValueHandler handler))
            {
                Debug.LogError($"Value '{valueReference}' is not registered.");
                return 0;
            }

            return handler.QueryValue();
        }
    }
}