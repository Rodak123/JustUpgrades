using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class GraphValueScaler : IValueScaler, IHasDynamicValues
    {
        [SerializeField] private FloatDynamicValue startValue = new(0);
        [SerializeField] private FloatDynamicValue endValue = new(10);
        [SerializeField] private AnimationCurve graph = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble startValueValue = startValue.GetFloatValue();
            BigDouble endValueValue = endValue.GetFloatValue();

            double graphValue = graph.Evaluate(Mathf.InverseLerp(level, 0, maxLevel));
            return startValueValue + graphValue * (endValueValue - startValueValue);
        }

        public string Hint() => "x = start + graph[level] * (end - start)";

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            yield return startValue;
            yield return endValue;
        }
    }
}