using System;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeReset
    {
        public AUpgradeContainerSO TargetUpgradeContainer;
        public IntDynamicValue ResetLevel = new(0, 0);
    }
}