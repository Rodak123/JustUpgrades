using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class Upgrade
    {
        private int currentLevel = 0;

        private readonly UpgradeSystemContext context;
        public readonly AUpgradeSO UpgradeSO;

        public int CurrentLevel => currentLevel;
        public bool IsInfinite => UpgradeSO.Level.IsInfinite.GetBoolValue(context.DynamicValuesModifiers);
        public int MaxLevel => IsInfinite ? currentLevel + 1 : UpgradeSO.Level.MaxLevel.GetIntValue(context.DynamicValuesModifiers);
        public int NextLevel => Mathf.Min(currentLevel + 1, MaxLevel);

        public event Action<Upgrade> OnLevelIncreased;

        public Upgrade(UpgradeSystemContext context, AUpgradeSO upgradeSO)
        {
            this.context = context;
            UpgradeSO = upgradeSO;
        }

        public void Update()
        {
            UpgradeSO.Update();
        }

        public bool TryUpgrade()
        {
            if (!CanBeUpgraded())
                return false;

            Dictionary<ACurrencySO, BigDouble> costs = GetCosts();
            if (!context.CurrencyWallet.TryPurchase(costs))
                return false;

            currentLevel++;
            UpgradeSO.ApplyUpgrade(currentLevel, context);
            OnLevelIncreased?.Invoke(this);
            return true;
        }

        public bool CanBeUpgraded()
        {
            if (!CanImprove())
                return false;

            if (!CanFitRequirements())
                return false;

            if (!CanAfford())
                return false;

            return true;
        }

        public bool CanImprove()
        {
            return currentLevel < MaxLevel;
        }

        public bool CanFitRequirements()
        {
            foreach (UpgradeRequirement requirement in UpgradeSO.Requirements)
            {
                if (!context.UpgradeRegistry.TryGetUpgrade(requirement.TargetUpgrade, out Upgrade requiredUpgrade))
                    return false;

                int targetLevel = requirement.UseMaxLevel.GetBoolValue(context.DynamicValuesModifiers)
                    ? requiredUpgrade.UpgradeSO.Level.MaxLevel.GetIntValue(context.DynamicValuesModifiers)
                    : requirement.TargetLevel.GetIntValue(context.DynamicValuesModifiers);

                if (requiredUpgrade.CurrentLevel < targetLevel)
                    return false;
            }
            return true;
        }

        public bool CanAfford()
        {
            Dictionary<ACurrencySO, BigDouble> costs = GetCosts();
            return context.CurrencyWallet.CanAfford(costs);
        }

        public Dictionary<ACurrencySO, BigDouble> GetCosts()
        {
            Dictionary<ACurrencySO, BigDouble> costDict = new();
            foreach (UpgradeCost cost in UpgradeSO.Costs)
            {
                ACurrencySO currency = cost.TargetCurrency;

                if (currency == null) continue;
                BigDouble costValue = cost.GetCostValue(currentLevel, MaxLevel);

                costDict.TryAdd(currency, 0);
                costDict[currency] += costValue;
            }
            return costDict;
        }
    }
}