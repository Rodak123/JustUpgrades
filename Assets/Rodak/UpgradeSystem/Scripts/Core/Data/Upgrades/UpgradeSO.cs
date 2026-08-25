using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Core/Upgrade", order = 100)]
    public class UpgradeSO : UniqueScriptableObject<UpgradeSO>
    {
        [SerializeField] private string upgradeName = "Upgrade Name";

        [SerializeField] private UpgradeLevel level;
        [SerializeField] private UpgradeCost[] costs;
        [SerializeField] private UpgradeRequirement[] requirements;
        public string UpgradeName => upgradeName;

        public UpgradeLevel Level => level;
        public UpgradeCost[] Costs => costs;
        public UpgradeRequirement[] Requirements => requirements;

        private void Reset()
        {
            upgradeName = name;
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