using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class AUpgradeSO : UniqueScriptableObject<AUpgradeSO>
    {
        [SerializeField] private string upgradeName = "Upgrade Name";
        [SerializeField] private UpgradeLevel level;
        [SerializeField] private UpgradeCost[] costs;
        [SerializeField] private UpgradeRequirement[] requirements;
        [SerializeReference] private AUpgradeParameter[] parameters;

        public string UpgradeName => upgradeName;
        public UpgradeLevel Level => level;
        public UpgradeCost[] Costs => costs;
        public UpgradeRequirement[] Requirements => requirements;

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