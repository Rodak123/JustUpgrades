using System;
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
        public bool IsInfinite => UpgradeSO.Level.IsInfinite.GetBoolValue();
        public int MaxLevel => IsInfinite ? currentLevel + 1 : UpgradeSO.Level.MaxLevel.GetIntValue();
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

            foreach (UpgradeCost cost in UpgradeSO.Costs)
            {
                ACurrencySO currency = cost.TargetCurrency;

                if (currency == null) continue;
                BigDouble costValue = cost.GetCostValue(currentLevel, MaxLevel);
                bool isSpent = cost.IsSpent.GetBoolValue();

                if (isSpent && !context.CurrencyWallet.TrySpend(currency, costValue))
                    throw new Exception($"Failed to purchase upgrade '{UpgradeSO}'");
            }

            currentLevel++;
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

                int targetLevel = requirement.UseMaxLevel.GetBoolValue()
                    ? requiredUpgrade.UpgradeSO.Level.MaxLevel.GetIntValue()
                    : requirement.TargetLevel.GetIntValue();

                if (requiredUpgrade.CurrentLevel < targetLevel)
                    return false;
            }
            return true;
        }

        public bool CanAfford()
        {
            foreach (UpgradeCost cost in UpgradeSO.Costs)
            {
                ACurrencySO currency = cost.TargetCurrency;

                if (currency == null) continue;
                BigDouble costValue = cost.GetCostValue(currentLevel, MaxLevel);

                if (!context.CurrencyWallet.CanSpend(currency, costValue))
                    return false;
            }
            return true;
        }
    }
}