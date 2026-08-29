using System;
using UnityEngine;
using Rodak.UpgradeSystem.Utils;
using System.Collections.Generic;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeLevel : IHasDynamicValues
    {
        [SerializeField] private IntDynamicValue maxLevel = new(1, 1);
        [SerializeField] private BoolDynamicValue isInfinite = new(false);

        public IntDynamicValue MaxLevel => maxLevel;
        public BoolDynamicValue IsInfinite => isInfinite;

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return maxLevel;
            yield return isInfinite;
        }
    }
}