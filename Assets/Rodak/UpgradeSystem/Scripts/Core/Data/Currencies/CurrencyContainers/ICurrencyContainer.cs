using System.Collections.Generic;

namespace Rodak.UpgradeSystem.Core
{
    public interface ICurrencyContainer
    {
        public IEnumerable<ACurrencySO> GetCurrencySOs();
    }
}