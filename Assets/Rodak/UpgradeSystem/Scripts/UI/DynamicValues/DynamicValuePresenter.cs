using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.UI
{
    public static class DynamicValuePresenter
    {
        public static string GetFormattedDynamicValue(BigDouble value)
        {
            string valueSign = value > 0 ? "+" : (value < 0 ? "-" : "");
            return $"{valueSign}{value}";
        }
    }
}