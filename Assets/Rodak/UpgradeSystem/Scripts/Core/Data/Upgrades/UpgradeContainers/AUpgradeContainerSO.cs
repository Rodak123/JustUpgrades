using System.Collections;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class AUpgradeContainerSO : UniqueScriptableObject<AUpgradeContainerSO>, IUpgradeContainer, IEnumerable<AUpgradeSO>
    {
        public abstract IEnumerable<AUpgradeSO> GetUpgradeSOs();

        public IEnumerator<AUpgradeSO> GetEnumerator()
        {
            return GetUpgradeSOs().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}