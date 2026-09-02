using System.Collections;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class ACurrencyContainerSO : UniqueScriptableObject<ACurrencyContainerSO>, ICurrencyContainer, IEnumerable<ACurrencySO>
    {
        public abstract IEnumerable<ACurrencySO> GetCurrencySOs();

        public IEnumerator<ACurrencySO> GetEnumerator()
        {
            return GetCurrencySOs().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}