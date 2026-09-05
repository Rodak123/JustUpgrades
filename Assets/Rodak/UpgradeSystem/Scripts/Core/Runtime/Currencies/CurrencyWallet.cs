using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class CurrencyWallet : MonoBehaviour, IHasSaveState
    {
        private class SaveState
        {
            public Dictionary<string, JObject> CurrencySaveStates;
        }

        public struct OnCurrencyChangedArgs
        {
            public ACurrencySO ACurrencySO;
            public BigDouble Amount;
        }

        [Serializable]
        private struct CurrencyGroup
        {
            public string groupName;
            public List<ACurrencyContainerSO> currencies;
        }

        [Header("Grouping only to organize")]
        [SerializeField] private List<CurrencyGroup> currencyGroups = new();

        [Header("Debug")]
        [SerializeField] private bool showWarnings = true;

        private readonly Dictionary<ACurrencySO, BigDouble> currencyAmounts = new();

        public event Action<OnCurrencyChangedArgs> OnCurrencyChanged;

        private bool isInitialized;

        public void Init()
        {
            for (int i = 0; i < currencyGroups.Count; i++)
            {
                List<ACurrencyContainerSO> currencyContainers = currencyGroups[i].currencies;
                for (int j = 0; j < currencyContainers.Count; j++)
                {
                    foreach (ACurrencySO currency in currencyContainers[j])
                    {
                        if (currency == null)
                        {
                            Debug.LogWarning($"Null currency at [{j}] in group: '{currencyGroups[i].groupName}'");
                            continue;
                        }
                        currencyAmounts.Add(currency, currency.BaseValue);
                    }
                }
            }

            isInitialized = true;
        }

        public void SetCurrency(ACurrencySO currency, BigDouble amount)
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
                ACurrencySO = currency,
                Amount = amount,
            });
        }

        public void AddCurrency(ACurrencySO currency, BigDouble amount)
        {
            BigDouble previousAmount = GetCurrencyAmount(currency);
            SetCurrency(currency, previousAmount + amount);
        }

        public BigDouble GetCurrencyAmount(ACurrencySO currency)
        {
            if (currencyAmounts.TryGetValue(currency, out BigDouble amount))
            {
                return amount;
            }
            if (showWarnings) Debug.LogWarning($"Accessing currency that is not registered: '{currency}'. Consider adding it to the Currencies list.");
            return 0;
        }

        public bool CanSpend(ACurrencySO currency, BigDouble costValue)
        {
            BigDouble storedAmount = GetCurrencyAmount(currency);
            if (storedAmount < costValue)
                return false;
            return true;
        }

        public bool TrySpend(ACurrencySO currency, BigDouble costValue)
        {
            if (!CanSpend(currency, costValue)) return false;
            currencyAmounts[currency] -= costValue;
            OnCurrencyChanged?.Invoke(new()
            {
                ACurrencySO = currency,
                Amount = currencyAmounts[currency],
            });
            return true;
        }

        public bool CanAfford(Dictionary<ACurrencySO, BigDouble> costs)
        {
            foreach (ACurrencySO currency in costs.Keys)
            {
                BigDouble costValue = costs[currency];
                BigDouble storedAmount = GetCurrencyAmount(currency);
                if (storedAmount < costValue)
                    return false;
            }
            return true;
        }

        public bool TryPurchase(Dictionary<ACurrencySO, BigDouble> costs)
        {
            if (!CanAfford(costs)) return false;
            foreach (ACurrencySO currency in costs.Keys)
            {
                BigDouble costValue = costs[currency];
                currencyAmounts[currency] -= costValue;

                OnCurrencyChanged?.Invoke(new()
                {
                    ACurrencySO = currency,
                    Amount = currencyAmounts[currency],
                });
            }
            return true;
        }

        public JObject CreateSaveState()
        {
            SaveState saveState = new()
            {
                CurrencySaveStates = new(),
            };

            foreach (ACurrencySO currencySO in currencyAmounts.Keys)
            {
                JObject currencySaveState = currencyAmounts[currencySO].CreateSaveState();
                saveState.CurrencySaveStates.Add(currencySO.SaveId, currencySaveState);
            }

            return JObject.FromObject(saveState);
        }

        public void LoadSaveState(JObject saveStateObject)
        {
            if (!isInitialized)
            {
                Debug.LogError($"Can't load state before initialization");
                return;
            }

            if (saveStateObject == null) return;

            SaveState saveState = saveStateObject.ToObject<SaveState>();

            if (saveState == null)
            {
                Debug.LogError($"Trying to load invalid save state: '{saveStateObject}'");
                return;
            }

            foreach (string currencyKey in saveState.CurrencySaveStates.Keys)
            {
                JObject currencySaveState = saveState.CurrencySaveStates[currencyKey];

                ACurrencySO targetCurrencySO = null;
                foreach (ACurrencySO currencySO in currencyAmounts.Keys)
                {
                    if (currencySO.SaveId == currencyKey)
                    {
                        targetCurrencySO = currencySO;
                        break;
                    }
                }

                if (targetCurrencySO == null)
                {
                    Debug.LogWarning($"Failed to load currency: '{currencyKey}', currency not registered, ignoring");
                    continue;
                }

                BigDouble savedValue = 0;
                savedValue.LoadSaveState(currencySaveState);

                SetCurrency(targetCurrencySO, savedValue);
            }
        }
    }
}