using System;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ExponentialValueScaler : IValueScaler
    {
        [SerializeField] private BigDouble baseValue = new(1);
        [SerializeField] private BigDouble multiplier = new(1.15);
        [SerializeField] private BigDouble multiplierGrow = new(0);

        public BigDouble GetValue(int level, int maxLevel)
        {
            return baseValue * BigDouble.Pow(multiplier + multiplierGrow * level, level);
        }

        public string Hint() => "x = base * (multiplier + (grow * level)) ^ level";
    }
}