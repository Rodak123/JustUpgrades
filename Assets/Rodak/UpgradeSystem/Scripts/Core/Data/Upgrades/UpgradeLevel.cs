using System;
using UnityEngine;
using Rodak.UpgradeSystem.Utils;
using System.Collections.Generic;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeLevel : IHasDynamicValues
    {
        [SerializeField] public IntDynamicValue MaxLevel = new(1, 1);
        [SerializeField] public BoolDynamicValue IsInfinite = new(false);

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return MaxLevel;
            yield return IsInfinite;
        }
    }
}