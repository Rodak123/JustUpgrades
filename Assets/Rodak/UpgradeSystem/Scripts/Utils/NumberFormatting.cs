using System;
using System.Globalization;
using UnityEngine;

namespace Rodak.UpgradeSystem.Utils
{
    public static class NumberFormatting
    {
        private static readonly NumberFormatInfo SpaceSeparatorFormat = new()
        {
            NumberGroupSeparator = " ",
            NumberDecimalDigits = 2
        };

        private static readonly string[] suffixes = { "", "k", "M", "B", "T", "Q", "Qa", "Qi", "Si", "Sp", "Oc", "No" };

        public static string GetCost(double value, bool floor = true)
        {
            double cost = floor ? Math.Floor(value) : value;

            if (cost < 1_000_000)
                return cost.ToString("N2", SpaceSeparatorFormat).TrimEnd('0').TrimEnd('.');

            double num = cost;
            int suffixIndex = 0;

            while (num >= 1_000 && suffixIndex < suffixes.Length - 1)
            {
                num /= 1_000.0;
                suffixIndex++;
            }

            return $"{num:0.##} {suffixes[suffixIndex]}";
        }

        public static string GetPercentage(float value)
        {
            return $"{Mathf.RoundToInt(value * 100)}%";
        }

        public static string GetFloat(float value)
        {
            return $"{Mathf.RoundToInt(value * 100f) / 100f}";
        }
    }
}