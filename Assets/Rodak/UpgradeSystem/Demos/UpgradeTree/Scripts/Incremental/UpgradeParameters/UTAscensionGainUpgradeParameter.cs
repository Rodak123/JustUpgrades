using System;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [Serializable, CreateUpgradeParameterMenu(Order = 110, MenuName = "UT/Ascension Gain")]
    public class UTAscensionGainUpgradeParameter : AUpgradeParameter
    {
        [SerializeField] private UTCurrencySO scalerCurrency;
        [SerializeField] private UTCurrencySO gainedCurrency;

        private Upgrade upgrade;

        private void Upgrade_OnUpgraded(Upgrade upgrade)
        {
            UpgradeSystemContext upgradeContext = UpgradeSystemContext.Instance;

            BigDouble scaler = upgradeContext.CurrencyWallet.GetCurrencyAmount(scalerCurrency);

            BigDouble gain = CalculateGain(scaler);

            upgradeContext.CurrencyWallet.AddCurrency(gainedCurrency, gain);
        }

        protected virtual BigDouble CalculateGain(BigDouble scaler)
        {
            return BigDouble.Sqrt(scaler);
        }

        public override void Init(AUpgradeSO upgradeSO)
        {
            UpgradeSystemContext.Instance.UpgradeTracker.TryGetUpgrade(upgradeSO, out upgrade);
            upgrade.OnUpgraded += Upgrade_OnUpgraded;
        }

        public override void Update() { }

        public override string GetInfoText()
        {
            return string.Empty;
        }

        public override string GetEditorDescription()
        {
            return $"Gains '{gainedCurrency}' using '{scalerCurrency}'";
        }
    }
}