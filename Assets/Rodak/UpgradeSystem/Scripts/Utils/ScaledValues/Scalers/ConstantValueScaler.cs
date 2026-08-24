using System;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ConstantValueScaler : IValueScaler
    {
        [SerializeField] private FloatDynamicValue value = new(1);

        [Tooltip("When true, the level 0 value is 0")]
        [SerializeField] private bool ignoredAtZero = false;

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble valueValue = value.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);

            if (ignoredAtZero && level == 0) return 0;
            return valueValue;
        }

        public string Hint() => "x = value";
    }
}