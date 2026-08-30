using System;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [Serializable]
    public class UTUpgradeVisual
    {
        private const string CURRENCY_SHORTCODE_FORMAT = "{C0}";
        private const string UPGRADE_SHORTCODE_FORMAT = "{U0}";
        private const string DYNAMIC_VALUE_SHORTCODE_FORMAT = "{V0}";
        private const string PARAMETER_SHORTCODE_FORMAT = "{P0}";

        private static readonly Color MentionedUpgradeColor = new(0.7f, 0.7f, 0.7f);

        [Header(
            "Description Shortcodes:\n" +
            PARAMETER_SHORTCODE_FORMAT + " - parameters\n" +
            CURRENCY_SHORTCODE_FORMAT + " - currencies\n" +
            UPGRADE_SHORTCODE_FORMAT + " - upgrades\n" +
            DYNAMIC_VALUE_SHORTCODE_FORMAT + " - dynamic values"
        )]
        [SerializeField, TextArea(4, 16)] private string upgradeDescription = "Description";
        [Space]
        [SerializeField] private UTCurrencySO[] mentionedCurrencies;
        [SerializeField] private UTUpgradeSO[] mentionedUpgrades;
        [SerializeField] private ADynamicValueReferenceSO[] mentionedDynamicValues;

        public string FormatDescription(UTUpgradeSO upgradeSO)
        {
            string description = upgradeDescription;

            for (int i = 0; i < mentionedCurrencies.Length; i++)
            {
                UTCurrencySO currency = mentionedCurrencies[i];
                if (currency == null) continue;
                string replace = CURRENCY_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, currency.ToString());
            }

            for (int i = 0; i < mentionedUpgrades.Length; i++)
            {
                UTUpgradeSO upgrade = mentionedUpgrades[i];
                if (upgrade == null) continue;
                string replace = UPGRADE_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, upgrade.ToString().WrapInColor(MentionedUpgradeColor));
            }

            for (int i = 0; i < mentionedDynamicValues.Length; i++)
            {
                ADynamicValueReferenceSO reference = mentionedDynamicValues[i];
                if (reference == null) continue;
                string replace = DYNAMIC_VALUE_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, reference.GetValueString());
            }

            for (int i = 0; i < mentionedDynamicValues.Length; i++)
            {
                ADynamicValueReferenceSO reference = mentionedDynamicValues[i];
                if (reference == null) continue;
                string replace = DYNAMIC_VALUE_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, reference.GetValueString());
            }

            for (int i = 0; i < upgradeSO.Parameters.Parameters.Length; i++)
            {
                AUpgradeParameter parameter = upgradeSO.Parameters.Parameters[i];
                string replace = PARAMETER_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, parameter.GetInfoText());
            }

            return description;
        }
    }
}