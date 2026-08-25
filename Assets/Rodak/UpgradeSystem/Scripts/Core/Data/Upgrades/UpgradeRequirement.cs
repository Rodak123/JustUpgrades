using System;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeRequirement
    {
        [SerializeField] private UpgradeSO targetUpgrade;
        [SerializeField] private BoolDynamicValue useMaxLevel;
        [SerializeField] private IntDynamicValue targetLevel = new(1);

        public UpgradeSO TargetUpgrade => targetUpgrade;
        public BoolDynamicValue UseMaxLevel => useMaxLevel;
        public IntDynamicValue TargetLevel => targetLevel;
    }
}