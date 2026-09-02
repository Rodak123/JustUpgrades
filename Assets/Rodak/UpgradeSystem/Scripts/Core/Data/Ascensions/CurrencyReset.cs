using System;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class CurrencyReset
    {
        public ACurrencyContainerSO TargetCurrencyContainer;
        public FloatDynamicValue ResetAmount = new(0, 0);
    }
}