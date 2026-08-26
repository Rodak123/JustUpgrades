using Rodak.UpgradeSystem.Core;

namespace Rodak.UpgradeSystem.UI
{
    public interface IUpgradeValueShortcode
    {
        public string ReplaceShortcodes(string text, Upgrade upgrade);
    }
}