using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using TMPro;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    public class UTCurrencyUI : MonoBehaviour
    {
        public UTCurrencyGenerator TargetGenerator;

        [Header("UI")]
        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private TMP_Text linearGainText;
        [SerializeField] private TMP_Text exponentialGainText;
        [SerializeField] private TMP_Text gainText;

        private UpgradeSystemContext upgradeContext;

        private void Start()
        {
            upgradeContext = UpgradeSystemContext.Instance;
            UpdateUI();

            upgradeContext.CurrencyWallet.OnCurrencyChanged += CurrencyWallet_OnCurrencyChanged;
        }

        private void CurrencyWallet_OnCurrencyChanged(CurrencyWallet.OnCurrencyChangedArgs args)
        {
            UpdateUI();
        }

        protected void UpdateUI()
        {
            BigDouble amount = upgradeContext.CurrencyWallet.GetCurrencyAmount(TargetGenerator.TargetCurrency);

            container.SetActive(TargetGenerator.LastUnlockedStatus && amount > 0);

            nameText.text = $"{CurrencyPresenter.GetFormattedCurrency(TargetGenerator.TargetCurrency)}:";
            amountText.text = CurrencyPresenter.WrapInCurrencyColor(amount.ToString(), TargetGenerator.TargetCurrency);

            linearGainText.text = $"{TargetGenerator.LastLinearGain.ToNiceString()}x";
            exponentialGainText.text = $"^{TargetGenerator.LastExponentialGain.ToNiceString()}";

            string gainSign = TargetGenerator.LastGain > 0 ? "+" : (TargetGenerator.LastGain < 0 ? "-" : "");
            gainText.text = TargetGenerator.LastGain == 0 ? "" : $"({gainSign}{TargetGenerator.LastGain.ToNiceString()}/s)";
        }
    }
}