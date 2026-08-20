using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ListValueScaler : IValueScaler
    {
        [SerializeField] private List<BigDouble> values;

        [Tooltip("When true, the sum of all previous values is added")]
        [SerializeField] private bool additive;

        public BigDouble GetValue(int level, int maxLevel)
        {
            if (level == 0 || values.Count == 0) return 0;
            int index = Math.Clamp(level - 1, 0, values.Count - 1);

            if (!additive) return values[index];

            BigDouble sum = 0;
            for (int i = 0; i <= index; i++)
            {
                sum += values[i];
            }
            return sum;
        }

        public string Hint() => "x = list[level - 1]";
    }
}