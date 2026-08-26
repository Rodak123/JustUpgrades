using UnityEngine;
using System;
using System.Collections.Generic;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ScaledValue : IHasDynamicValues
    {
        [SerializeReference] private IValueScaler scaler = null;

        public ScaledValue()
        {
            scaler = null;
        }

        public bool HasValue => scaler != null;

        public BigDouble GetValue(int level, int maxLevel)
        {
            if (scaler == null) return 0;
            if (level < 0) level = 0;
            if (maxLevel < 0) maxLevel = 0;
            return scaler.GetValue(level, maxLevel);
        }

        public IEnumerable<ADynamicValueReferenceSO> GetDynamicValueReferences()
        {
            if (scaler is IHasDynamicValues hasDynamicValues)
            {
                foreach (ADynamicValueReferenceSO reference in hasDynamicValues.GetDynamicValueReferences())
                {
                    yield return reference;
                }
            }
        }

    }
}