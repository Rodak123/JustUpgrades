using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Demo UT/Upgrade", order = 110)]
    public class UTUpgradeSO : AUpgradeSO
    {
        [SerializeField] private BoolDynamicValue isAutomated = new(false);

        [Header("Appearance")]
        [SerializeField] private string upgradeNumber;
        [SerializeField] private UTUpgradeVisual visual;

        public BoolDynamicValue IsAutomated => isAutomated;

        public string UpgradeNumber => upgradeNumber;
        public string UpgradeDescription => visual.FormatDescription(this);

        public override IEnumerable<IDynamicValue> GetDynamicValues()
        {
            foreach (IDynamicValue dynamicValue in base.GetDynamicValues())
            {
                yield return dynamicValue;
            }
            yield return isAutomated;
        }
    }
}