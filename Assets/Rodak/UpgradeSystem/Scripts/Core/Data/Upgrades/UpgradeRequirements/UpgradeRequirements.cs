using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeRequirements : IHasDynamicValues
    {
        [SerializeReference] private List<AUpgradeRequirement> requirements;

        public IReadOnlyList<AUpgradeRequirement> Requirements => requirements;

        public bool FitsRequirements(AUpgradeSO upgradeSO)
        {
            foreach (AUpgradeRequirement requirement in requirements)
            {
                bool fits = requirement.FitsRequirement(upgradeSO);
                if (!fits)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            foreach (AUpgradeRequirement requirement in requirements)
            {
                if (requirement is not IHasDynamicValues hasDynamicValues) continue;
                foreach (IDynamicValue dynamicValue in hasDynamicValues.GetDynamicValues())
                {
                    yield return dynamicValue;
                }
            }
        }
    }
}