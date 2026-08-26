using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class AUpgradeSO : UniqueScriptableObject<AUpgradeSO>, IHasDynamicValues
    {
        [SerializeField] private string upgradeName = "Upgrade Name";

        [Space]
        [SerializeField] private UpgradeLevel level;
        [SerializeField] private UpgradeCost[] costs;
        [SerializeField] private UpgradeRequirement[] requirements;
        [SerializeReference] private AUpgradeParameter[] parameters;

        public string UpgradeName => upgradeName;

        public UpgradeLevel Level => level;
        public UpgradeCost[] Costs => costs;
        public UpgradeRequirement[] Requirements => requirements;
        public AUpgradeParameter[] Parameters => parameters;

        public void Init(UpgradeSystemContext upgradeContext)
        {
            foreach (AUpgradeParameter parameter in parameters)
            {
                parameter.Init(upgradeContext, this);
            }
        }

        public void ApplyUpgrade(int level, UpgradeSystemContext context)
        {
            foreach (AUpgradeParameter parameter in parameters)
            {
                parameter.ApplyParameterUpgrade(level, context);
            }
        }

        public void Update()
        {
            foreach (AUpgradeParameter parameter in parameters)
            {
                parameter.Update();
            }
        }

        public IEnumerable<ADynamicValueReferenceSO> GetDynamicValueReferences()
        {
            foreach (ADynamicValueReferenceSO reference in level.GetDynamicValueReferences())
            {
                yield return reference;
            }
            foreach (UpgradeCost cost in costs)
            {
                foreach (ADynamicValueReferenceSO reference in cost.GetDynamicValueReferences())
                {
                    yield return reference;
                }
            }
            foreach (UpgradeRequirement requirement in requirements)
            {
                foreach (ADynamicValueReferenceSO reference in requirement.GetDynamicValueReferences())
                {
                    yield return reference;
                }
            }
            foreach (AUpgradeParameter parameter in parameters)
            {
                if (parameter is not IHasDynamicValues hasDynamicValues) continue;
                foreach (ADynamicValueReferenceSO reference in hasDynamicValues.GetDynamicValueReferences())
                {
                    yield return reference;
                }
            }
        }

        public override string ToString()
        {
            return upgradeName;
        }

        public string GetChangingValueSuffix()
        {
            return $"upgrade:{UniqueId}";
        }
    }
}