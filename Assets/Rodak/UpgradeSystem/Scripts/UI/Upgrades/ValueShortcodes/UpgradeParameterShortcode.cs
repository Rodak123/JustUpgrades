using Rodak.UpgradeSystem.Core;

namespace Rodak.UpgradeSystem.UI
{
    public class UpgradeParameterShortcode : IUpgradeValueShortcode
    {
        private const string Shortcode = "%PARAM";

        public string ReplaceShortcodes(string text, Upgrade upgrade)
        {
            for (int i = 0; i < upgrade.UpgradeSO.Parameters.Length; i++)
            {
                AUpgradeParameter parameter = upgrade.UpgradeSO.Parameters[i];

                string formattedParameter = parameter.GetInfoText(upgrade.CurrentLevel);
                text = text.Replace($"{Shortcode}[{i}]", formattedParameter);
            }
            return text;
        }
    }
}