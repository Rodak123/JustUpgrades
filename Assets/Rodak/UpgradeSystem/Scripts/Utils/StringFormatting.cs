using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    public static class StringFormatting
    {
        public static string WrapInColor(this string text, Color? color)
        {
            if (color.HasValue) return text.WrapInColor(color.Value);
            return text;
        }

        public static string WrapInColor(this string text, Color color)
        {
            string hex = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{hex}>{text}</color>";
        }
    }
}