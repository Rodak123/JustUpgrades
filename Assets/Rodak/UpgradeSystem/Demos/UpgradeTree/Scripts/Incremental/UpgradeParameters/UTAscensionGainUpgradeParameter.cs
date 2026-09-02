using System;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.UI;
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

        public BigDouble CurrentGain
        {
            get
            {
                BigDouble scaler = UpgradeSystemContext.Instance.CurrencyWallet.GetCurrencyAmount(scalerCurrency);
                BigDouble gain = CalculateGain(scaler);
                return gain;
            }
        }

        private void Upgrade_OnUpgraded(Upgrade upgrade)
        {
            UpgradeSystemContext.Instance.CurrencyWallet.AddCurrency(gainedCurrency, CurrentGain);
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
            return $"Current: {CurrencyPresenter.GetFormattedCurrency(gainedCurrency, CurrentGain, false)}";
        }

        public override string GetEditorDescription()
        {
            return $"Gains '{gainedCurrency}' using '{scalerCurrency}'";
        }
    }
}