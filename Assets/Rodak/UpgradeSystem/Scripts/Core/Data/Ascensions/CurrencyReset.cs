using System;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class CurrencyReset
    {
        public ACurrencySO TargetCurrency;
        public FloatDynamicValue ResetAmount = new(0, 0);
    }
}