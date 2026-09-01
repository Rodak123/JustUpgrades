using System;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeReset
    {
        public AUpgradeSO TargetUpgrade;
        public IntDynamicValue ResetLevel = new(0, 0);
    }
}