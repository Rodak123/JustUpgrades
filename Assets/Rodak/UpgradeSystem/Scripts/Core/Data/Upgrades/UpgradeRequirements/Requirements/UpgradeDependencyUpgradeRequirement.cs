using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable, CreateUpgradeRequirementMenu(Order = 100, MenuName = "Upgrade Dependency")]
    public class UpgradeDependencyUpgradeRequirement : AUpgradeRequirement, IHasDynamicValues
    {
        [SerializeField] private AUpgradeSO targetUpgradeSO;
        [SerializeField] private BoolDynamicValue useMaxLevel = new(false);
        [SerializeField] private IntDynamicValue targetLevel = new(1, 0);

        public AUpgradeSO TargetUpgrade => targetUpgradeSO;
        public BoolDynamicValue UseMaxLevel => useMaxLevel;
        public IntDynamicValue TargetLevel => targetLevel;

        public override bool FitsRequirement(AUpgradeSO upgradeSO)
        {
            UpgradeSystemContext upgradeContext = UpgradeSystemContext.Instance;

            if (targetUpgradeSO == null || !upgradeContext.UpgradeTracker.TryGetUpgrade(targetUpgradeSO, out Upgrade targetUpgrade))
            {
                return false;
            }

            int actualTargetLevel = useMaxLevel.GetBoolValue()
                ? targetUpgrade.MaxLevel
                : targetLevel.GetIntValue();

            return targetUpgrade.CurrentLevel >= actualTargetLevel;
        }

        public override string GetInfoText()
        {
            if (targetUpgradeSO == null) return $"Missing '{nameof(targetUpgradeSO)}'!";

            bool useMaxLevelValue = useMaxLevel.GetBoolValue();
            bool isInfiniteValue = targetUpgradeSO.Level.IsInfinite.GetBoolValue();
            int targetLevelValue = targetLevel.GetIntValue();

            string targetLevelString = (useMaxLevelValue && !isInfiniteValue) ? "Max" : targetLevelValue.ToString();
            return $"lv. {targetLevelString} {UpgradePresenter.GetFormattedName(targetUpgradeSO)}";
        }

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return useMaxLevel;
            yield return targetLevel;
        }

        public override string GetEditorDescription()
        {
            if (targetUpgradeSO == null) return $"Missing '{nameof(targetUpgradeSO)}'!";
            if (useMaxLevel.GetBoolValue()) return $"Needs '{targetUpgradeSO}' at MAX level";
            else return $"Needs '{targetUpgradeSO}' at level '{targetLevel.GetIntValue()}'";
        }
    }
}