using Rodak.UpgradeSystem.Core;

namespace Rodak.UpgradeSystem.UI
{
    public class UpgradeRequirementShortcode : IUpgradeValueShortcode
    {
        private const string Shortcode = "%REQ";

        public string ReplaceShortcodes(string text, Upgrade upgrade)
        {
            for (int i = 0; i < upgrade.UpgradeSO.Requirements.Length; i++)
            {
                UpgradeRequirement upgradeRequirement = upgrade.UpgradeSO.Requirements[i];
                string formattedRequirement = UpgradePresenter.GetFormattedRequirement(upgradeRequirement);
                text = text.Replace($"{Shortcode}[{i}]", formattedRequirement);
            }
            return text;
        }
    }
}