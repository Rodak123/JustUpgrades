using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class CurrencyWallet : MonoBehaviour
    {
        public struct OnCurrencyChangedArgs
        {
            public CurrencySO CurrencySO;
            public BigDouble Amount;
        }

        [Serializable]
        private struct CurrencyGroup
        {
            public string groupName;
            public List<CurrencySO> currencies;
        }

        [Header("Grouping only to organize")]
        [SerializeField] private List<CurrencyGroup> currencyGroups = new();

        [Header("Debug")]
        [SerializeField] private bool showWarnings = true;

        private readonly Dictionary<CurrencySO, BigDouble> currencyAmounts = new();

        public event Action<OnCurrencyChangedArgs> OnCurrencyChanged;

        public void Init()
        {
            for (int i = 0; i < currencyGroups.Count; i++)
            {
                List<CurrencySO> currencies = currencyGroups[i].currencies;
                for (int j = 0; j < currencies.Count; j++)
                {
                    CurrencySO currency = currencies[j];
                    if (currency == null)
                    {
                        Debug.LogWarning($"Null currency at [{j}] in group: '{currencyGroups[i].groupName}'");
                        continue;
                    }
                    currencyAmounts.Add(currency, currency.BaseValue);
                }
            }
        }

        public void SetCurrency(CurrencySO currency, BigDouble amount)
        {
            if (currencyAmounts.TryGetValue(currency, out BigDouble currentAmount))
            {
                currencyAmounts[currency] = amount;
            }
            else
            {
                currencyAmounts.Add(currency, amount);
            }

            OnCurrencyChanged?.Invoke(new()
            {
                CurrencySO = currency,
                Amount = amount,
            });
        }

        public void AddCurrency(CurrencySO currency, BigDouble amount)
        {
            BigDouble previousAmount = GetCurrencyAmount(currency);
            SetCurrency(currency, previousAmount + amount);
        }

        public BigDouble GetCurrencyAmount(CurrencySO currency)
        {
            if (currencyAmounts.TryGetValue(currency, out BigDouble amount))
            {
                return amount;
            }
            if (showWarnings) Debug.LogWarning($"Accessing currency that is not registered: '{currency}'. Consider adding it to the Currencies list.");
            return 0;
        }

        public bool CanSpend(CurrencySO currency, BigDouble costValue)
        {
            BigDouble storedAmount = GetCurrencyAmount(currency);
            if (storedAmount < costValue)
                return false;
            return true;
        }

        public bool TrySpend(CurrencySO currency, BigDouble costValue)
        {
            if (!CanSpend(currency, costValue)) return false;
            currencyAmounts[currency] -= costValue;
            return true;
        }

        public bool CanAfford(Dictionary<CurrencySO, BigDouble> costs)
        {
            foreach (CurrencySO currency in costs.Keys)
            {
                BigDouble costValue = costs[currency];
                BigDouble storedAmount = GetCurrencyAmount(currency);
                if (storedAmount < costValue)
                    return false;
            }
            return true;
        }

        public bool TryPurchase(Dictionary<CurrencySO, BigDouble> costs)
        {
            if (!CanAfford(costs)) return false;
            foreach (CurrencySO currency in costs.Keys)
            {
                BigDouble costValue = costs[currency];
                currencyAmounts[currency] -= costValue;

                OnCurrencyChanged?.Invoke(new()
                {
                    CurrencySO = currency,
                    Amount = currencyAmounts[currency],
                });
            }
            return true;
        }
    }
}