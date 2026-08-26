using Rodak.UpgradeSystem.Core;

namespace Rodak.UpgradeSystem.UI
{
    public class NextUpgradeParameterShortcode : IUpgradeValueShortcode
    {
        private const string Shortcode = "%NPARAM";

        public string ReplaceShortcodes(string text, Upgrade upgrade)
        {
            for (int i = 0; i < upgrade.UpgradeSO.Parameters.Length; i++)
            {
                AUpgradeParameter parameter = upgrade.UpgradeSO.Parameters[i];

                string formattedParameter = parameter.GetInfoText(upgrade.NextLevel);
                text = text.Replace($"{Shortcode}[{i}]", formattedParameter);
            }
            return text;
        }
    }
}