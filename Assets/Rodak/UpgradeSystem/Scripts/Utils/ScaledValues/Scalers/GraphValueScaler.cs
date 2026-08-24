using System;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class GraphValueScaler : IValueScaler
    {
        [SerializeField] private FloatDynamicValue startValue = new(0);
        [SerializeField] private FloatDynamicValue endValue = new(10);
        [SerializeField] private AnimationCurve graph = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public BigDouble GetValue(int level, int maxLevel)
        {
            BigDouble startValueValue = startValue.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);
            BigDouble endValueValue = endValue.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);

            double graphValue = graph.Evaluate(Mathf.InverseLerp(level, 0, maxLevel));
            return startValueValue + graphValue * (endValueValue - startValueValue);
        }

        public string Hint() => "x = start + graph[level] * (end - start)";
    }
}