using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Demo UT/Upgrade", order = 110)]
    public class UTUpgradeSO : AUpgradeSO
    {
        [SerializeField] private int upgradeNumber;
        [SerializeField] private UTUpgradeVisual upgradeVisual;

        public int UpgradeNumber => upgradeNumber;
        public UTUpgradeVisual UpgradeVisual => upgradeVisual;
    }
}