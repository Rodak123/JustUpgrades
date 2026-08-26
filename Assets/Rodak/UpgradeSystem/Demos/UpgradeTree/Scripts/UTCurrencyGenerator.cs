using System;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    /**
        Uses the linear and exponential upgradable values to increment the target currency.
    */
    public class UTCurrencyGenerator : MonoBehaviour
    {
        [Serializable]
        public struct CurrencyBoost
        {
            [SerializeField] private UTCurrencySO targetCurrency;
            [SerializeField] private FloatDynamicValue targetBoost;

            public readonly UTCurrencySO TargetCurrency => targetCurrency;
            public readonly FloatDynamicValue TargetBoost => targetBoost;
        }

        [Header("Currency")]
        [SerializeField] private UTCurrencySO targetCurrency;

        [Header("Gains")]
        [SerializeField] private BoolDynamicValue targetUnlockedStatus = new(true);
        [SerializeField] private FloatDynamicValue targetSingleUseGain;
        [SerializeField] private FloatDynamicValue targetLinearGain;
        [SerializeField] private FloatDynamicValue targetExponentialGain;

        [Header("Boosts")]
        [Tooltip("These use the currency value to boost this currency gain")]
        [SerializeField] private CurrencyBoost[] linearCurrencyBoosts;
        [SerializeField] private CurrencyBoost[] exponentialCurrencyBoosts;

        private UpgradeSystemContext upgradeContext;

        public UTCurrencySO TargetCurrency => targetCurrency;

        public bool LastUnlockedStatus { get; private set; }
        public BigDouble LastTotalSingleUseGain { get; private set; }
        public BigDouble LastLinearGain { get; private set; }
        public BigDouble LastExponentialGain { get; private set; }
        public BigDouble LastGain { get; private set; }

        private void Start()
        {
            upgradeContext = UpgradeSystemContext.Instance;
        }

        private void Update()
        {
            LastUnlockedStatus = targetUnlockedStatus.GetBoolValue(upgradeContext.DynamicValuesModifiers);

            if (LastUnlockedStatus == false) return;

            // Single
            BigDouble totalSingleUseGain = targetSingleUseGain.GetFloatValue(upgradeContext.DynamicValuesModifiers);
            BigDouble singleUseGain = totalSingleUseGain - LastTotalSingleUseGain;
            LastTotalSingleUseGain = totalSingleUseGain;
            upgradeContext.CurrencyWallet.AddCurrency(targetCurrency, singleUseGain);

            // Automated
            BigDouble previousAmount = upgradeContext.CurrencyWallet.GetCurrencyAmount(targetCurrency);

            LastLinearGain = CalculateLinearGain();
            LastExponentialGain = CalculateExponentialGain();

            LastGain = targetCurrency.BaseGain * LastLinearGain + previousAmount * LastExponentialGain;

            BigDouble generated = LastGain * Time.deltaTime;
            upgradeContext.CurrencyWallet.AddCurrency(targetCurrency, generated);
        }

        private BigDouble CalculateLinearGain()
        {
            BigDouble linearGain = targetLinearGain.GetFloatValue(upgradeContext.DynamicValuesModifiers);

            foreach (CurrencyBoost currencyBoost in linearCurrencyBoosts)
            {
                BigDouble currencyValue = upgradeContext.CurrencyWallet.GetCurrencyAmount(currencyBoost.TargetCurrency);
                BigDouble boost = currencyBoost.TargetBoost.GetFloatValue(upgradeContext.DynamicValuesModifiers);
                linearGain += currencyValue * boost;
            }

            return linearGain;
        }

        private BigDouble CalculateExponentialGain()
        {
            BigDouble exponentialGain = targetExponentialGain.GetFloatValue(upgradeContext.DynamicValuesModifiers);

            foreach (CurrencyBoost currencyBoost in exponentialCurrencyBoosts)
            {
                BigDouble currencyValue = upgradeContext.CurrencyWallet.GetCurrencyAmount(currencyBoost.TargetCurrency);
                BigDouble boost = currencyBoost.TargetBoost.GetFloatValue(upgradeContext.DynamicValuesModifiers);
                exponentialGain += currencyValue * boost;
            }

            return exponentialGain;
        }

        public override string ToString()
        {
            return $"{CurrencyPresenter.GetFormattedCurrency(targetCurrency, LastLinearGain)}x ^{CurrencyPresenter.GetFormattedCurrency(targetCurrency, LastExponentialGain)}";
        }
    }
}