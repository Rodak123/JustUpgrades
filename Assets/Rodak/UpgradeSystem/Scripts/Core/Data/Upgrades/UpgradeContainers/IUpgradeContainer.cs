using System.Collections.Generic;

namespace Rodak.UpgradeSystem.Core
{
    public interface IUpgradeContainer
    {
        public IEnumerable<AUpgradeSO> GetUpgradeSOs();
    }
}