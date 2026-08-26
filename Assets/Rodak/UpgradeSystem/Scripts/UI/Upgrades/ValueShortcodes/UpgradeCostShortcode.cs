using Rodak.UpgradeSystem.Core;

namespace Rodak.UpgradeSystem.UI
{
    public class UpgradeCostShortcode : IUpgradeValueShortcode
    {
        private const string Shortcode = "%COST";

        public string ReplaceShortcodes(string text, Upgrade upgrade)
        {
            int maxLevel = upgrade.UpgradeSO.Level.MaxLevel.GetIntValue();

            for (int i = 0; i < upgrade.UpgradeSO.Costs.Length; i++)
            {
                UpgradeCost upgradeCost = upgrade.UpgradeSO.Costs[i];

                string formattedCost = UpgradePresenter.GetFormattedCost(upgradeCost, upgrade.CurrentLevel, maxLevel);
                text = text.Replace($"{Shortcode}[{i}]", formattedCost);
            }
            return text;
        }
    }
}