using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Demo UT/Upgrade", order = 110)]
    public class UTUpgradeSO : AUpgradeSO
    {
        [SerializeField] private string upgradeNumber;
        [SerializeField] private UTUpgradeVisual visual;

        public string UpgradeNumber => upgradeNumber;
        public string UpgradeDescription => visual.FormatDescription(this);
    }
}