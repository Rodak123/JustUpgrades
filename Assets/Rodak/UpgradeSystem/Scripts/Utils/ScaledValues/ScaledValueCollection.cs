using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ScaledValueCollection : IEnumerable<ScaledValue>
    {
        [SerializeField] private ScaledValue[] scaledValues;

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble sum = default;
            foreach (ScaledValue scaledValue in this)
            {
                sum += scaledValue.GetValue(level, maxLevel);
            }
            return sum;
        }

        public IEnumerator<ScaledValue> GetEnumerator()
        {
            foreach (ScaledValue scaledValue in scaledValues)
            {
                yield return scaledValue;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}