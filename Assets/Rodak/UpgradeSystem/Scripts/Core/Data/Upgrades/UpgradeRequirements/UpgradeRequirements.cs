using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeRequirements : IHasDynamicValues
    {
        [SerializeReference] private AUpgradeRequirement[] requirements;

        public AUpgradeRequirement[] Requirements => requirements;

        public bool FitsRequirements(UpgradeSystemContext context, AUpgradeSO upgradeSO)
        {
            foreach (AUpgradeRequirement requirement in requirements)
            {
                bool fits = requirement.FitsRequirement(context, upgradeSO);
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