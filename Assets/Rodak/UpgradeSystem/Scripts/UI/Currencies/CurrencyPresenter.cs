using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.UI
{
    public static class CurrencyPresenter
    {
        public static string WrapInCurrencyColor(string text, ACurrencySO currency)
        {
            if (currency == null) return text;
            return text.WrapInColor(currency.CurrencyColor);
        }

        public static string GetFormattedCurrency(ACurrencySO currency)
        {
            if (currency == null) return "None";
            return WrapInCurrencyColor(currency.CurrencyName, currency);
        }

        public static string GetFormattedCurrency(ACurrencySO currency, BigDouble value, bool includeName = true, Color? valueColor = null)
        {
            if (currency == null) return "None";
            string text = value.ToString().WrapInColor(valueColor);
            if (includeName) text += $" {currency.CurrencyName}";
            return WrapInCurrencyColor(text, currency);
        }
    }
}