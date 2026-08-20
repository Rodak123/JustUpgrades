using System;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ConstantValueScaler : IValueScaler
    {
        [SerializeField] private BigDouble value = new(1);

        [Tooltip("When true, the level 0 value is 0")]
        [SerializeField] private bool ignoredAtZero = false;

        public BigDouble GetValue(int level, int maxLevel)
        {
            if (ignoredAtZero && level == 0) return 0;
            return value;
        }

        public string Hint() => "x = value";
    }
}