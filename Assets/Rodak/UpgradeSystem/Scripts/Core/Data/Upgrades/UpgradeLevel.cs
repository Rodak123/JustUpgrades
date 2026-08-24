using System;
using UnityEngine;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public struct UpgradeLevel
    {
        [SerializeField] public IntDynamicValue MaxLevel;
        [SerializeField] public BoolDynamicValue IsInfinite;
    }
}