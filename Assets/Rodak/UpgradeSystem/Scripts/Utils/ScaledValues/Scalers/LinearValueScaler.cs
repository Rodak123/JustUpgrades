using System;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class LinearValueScaler : IValueScaler
    {
        [SerializeField] private BigDouble baseValue = new(0);
        [SerializeField] private BigDouble increment = new(1);
        [SerializeField] private BigDouble incrementGrow = new(0);

        public BigDouble GetValue(int level, int maxLevel)
        {
            return baseValue + (increment + (incrementGrow * level)) * level;
        }

        public string Hint() => "x = base + (increment + (grow * level)) * level";
    }
}