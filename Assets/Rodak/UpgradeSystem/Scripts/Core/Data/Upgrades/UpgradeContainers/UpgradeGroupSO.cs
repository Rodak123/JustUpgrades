using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Core/Upgrade Group", order = 100)]
    public class UpgradeGroupSO : AUpgradeContainerSO
    {
        [SerializeField] private List<AUpgradeSO> upgrades;

        public override IEnumerable<AUpgradeSO> GetUpgradeSOs()
        {
            return upgrades;
        }

        public override string ToString()
        {
            return $"UpgradeGroup[{upgrades.Count}]";
        }
    }
}