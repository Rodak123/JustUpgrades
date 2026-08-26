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
            container.SetActive(TargetGenerator.LastUnlockedStatus);

            BigDouble amount = upgradeContext.CurrencyWallet.GetCurrencyAmount(TargetGenerator.TargetCurrency);

            nameText.text = $"{CurrencyPresenter.GetFormattedCurrency(TargetGenerator.TargetCurrency)}:";
            amountText.text = CurrencyPresenter.WrapInCurrencyColor(amount.ToString(), TargetGenerator.TargetCurrency);

            linearGainText.text = $"{TargetGenerator.LastLinearGain:e3}x";
            exponentialGainText.text = $"^{TargetGenerator.LastExponentialGain:e3}";

            string gainSign = TargetGenerator.LastGain > 0 ? "+" : (TargetGenerator.LastGain < 0 ? "-" : "");
            gainText.text = $"({gainSign}{TargetGenerator.LastGain:e2}/s)";
        }
    }
}