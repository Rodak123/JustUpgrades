using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeRequirement : IHasDynamicValues
    {
        [SerializeField] private AUpgradeSO targetUpgrade;
        [SerializeField] private BoolDynamicValue useMaxLevel = new(false);
        [SerializeField] private IntDynamicValue targetLevel = new(1, 0);

        public AUpgradeSO TargetUpgrade => targetUpgrade;
        public BoolDynamicValue UseMaxLevel => useMaxLevel;
        public IntDynamicValue TargetLevel => targetLevel;

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return useMaxLevel;
            yield return targetLevel;
        }
    }
}