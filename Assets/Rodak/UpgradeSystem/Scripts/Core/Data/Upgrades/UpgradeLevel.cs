using System;
using UnityEngine;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeLevel
    {
        [SerializeField] public IntDynamicValue MaxLevel = new(1, 1);
        [SerializeField] public BoolDynamicValue IsInfinite = new(false);
    }
}