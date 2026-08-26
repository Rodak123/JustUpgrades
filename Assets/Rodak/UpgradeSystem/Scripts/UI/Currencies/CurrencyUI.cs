using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using TMPro;
using UnityEngine;

namespace Rodak.UpgradeSystem.UI
{
    public class CurrencyUI : MonoBehaviour
    {
        // public UpgradeSystemContext UpgradeContext;
        // public ACurrencySO TargetCurrency;

        // [Header("UI")]
        // [SerializeField] private TMP_Text nameText;
        // [SerializeField] private TMP_Text amountText;

        // private void Start()
        // {
        //     UpdateUI();
        //     UpgradeContext.CurrencyWallet.OnCurrencyChanged += CurrencyWallet_OnCurrencyChanged;
        // }

        // private void CurrencyWallet_OnCurrencyChanged(CurrencyWallet.OnCurrencyChangedArgs args)
        // {
        //     UpdateUI();
        // }

        // protected virtual void UpdateUI()
        // {
        //     BigDouble amount = UpgradeContext.CurrencyWallet.GetCurrencyAmount(TargetCurrency);

        //     nameText.text = $"{CurrencyPresenter.GetFormattedCurrency(TargetCurrency)}:";
        //     amountText.text = CurrencyPresenter.WrapInCurrencyColor(amount.ToString(), TargetCurrency);
        // }
    }
}