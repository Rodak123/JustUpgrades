using System;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class GraphValueScaler : IValueScaler
    {
        [SerializeField] private BigDouble startValue = new(0);
        [SerializeField] private BigDouble endValue = new(10);
        [SerializeField] private AnimationCurve graph = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public BigDouble GetValue(int level, int maxLevel)
        {
            double graphValue = graph.Evaluate(Mathf.InverseLerp(level, 0, maxLevel));
            return startValue + graphValue * (endValue - startValue);
        }

        public string Hint() => "x = start + graph[level] * (end - start)";
    }
}