using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class AAscension : UniqueScriptableObject<AAscension>
    {
        [SerializeField] private List<UpgradeReset> upgradeResets;
        [SerializeField] private List<CurrencyReset> currencyResets;

        public IReadOnlyList<UpgradeReset> UpgradeResets => upgradeResets;
        public IReadOnlyList<CurrencyReset> CurrencyResets => currencyResets;
    }
}