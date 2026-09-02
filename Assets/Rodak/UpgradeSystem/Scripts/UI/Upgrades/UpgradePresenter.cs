using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.UI
{
    public static class UpgradePresenter
    {
        public static string GetFormattedCost(UpgradeCost upgradeCost, int currentLevel, int maxLevel, bool includeName = true, Color? costNumberColor = null)
        {
            if (upgradeCost == null) return string.Empty;

            BigDouble costValue = upgradeCost.GetCostValue(currentLevel, maxLevel);
            bool isSpent = upgradeCost.IsSpent.GetBoolValue();

            return
            (isSpent ? "Cost: " : "Requires: ") +
            CurrencyPresenter.GetFormattedCurrency(upgradeCost.TargetCurrency, costValue, includeName, costNumberColor);
        }

        public static string GetFormattedRequirement(AUpgradeRequirement upgradeRequirement)
        {
            if (upgradeRequirement == null) return string.Empty;

            return upgradeRequirement.GetInfoText();
        }

        public static string GetFormattedName(AUpgradeSO upgradeSO)
        {
            if (upgradeSO == null) return "None";
            return upgradeSO.UpgradeName;
        }

        public static string GetFormattedLevel(Upgrade upgrade)
        {
            if (upgrade == null) return string.Empty;

            int current = upgrade.CurrentLevel;

            bool isInfinite = upgrade.UpgradeSO.Level.IsInfinite.GetBoolValue();
            if (isInfinite) return current.ToString();

            int maxLevel = upgrade.UpgradeSO.Level.MaxLevel.GetIntValue();
            return $"{current} / {maxLevel}";
        }

        public static string GetFormattedCosts(Upgrade upgrade, bool includeName = true, Color? costNumberColor = null)
        {
            if (upgrade == null || !upgrade.CanImprove()) return string.Empty;

            if (upgrade.UpgradeSO.Costs.Costs.Count == 0)
            {
                return "Free";
            }

            int maxLevel = upgrade.UpgradeSO.Level.MaxLevel.GetIntValue();

            List<string> costsList = new();
            foreach (UpgradeCost cost in upgrade.UpgradeSO.Costs.Costs)
            {
                costsList.Add(GetFormattedCost(cost, upgrade.CurrentLevel, maxLevel, includeName));
            }

            return string.Join("\n", costsList);
        }
    }
}