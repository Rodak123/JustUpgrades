using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [Serializable]
    public class UTUpgradeVisual
    {
        [Serializable]
        private struct Mentions
        {
            [Header(
                "Description Shortcodes:\n" +
                PARAMETER_SHORTCODE_FORMAT + " - parameters\n" +
                CURRENCY_SHORTCODE_FORMAT + " - currencies\n" +
                UPGRADE_SHORTCODE_FORMAT + " - upgrades\n" +
                DYNAMIC_VALUE_SHORTCODE_FORMAT + " - dynamic values"
            )]
            [SerializeField] private List<UTCurrencySO> mentionedCurrencies;
            [SerializeField] private List<UTUpgradeSO> mentionedUpgrades;
            [SerializeField] private List<ADynamicValueReferenceSO> mentionedDynamicValues;

            public IReadOnlyList<UTCurrencySO> MentionedCurrencies => mentionedCurrencies;
            public IReadOnlyList<UTUpgradeSO> MentionedUpgrades => mentionedUpgrades;
            public IReadOnlyList<ADynamicValueReferenceSO> MentionedDynamicValues => mentionedDynamicValues;
        }

        private const string CURRENCY_SHORTCODE_FORMAT = "{C0}";
        private const string UPGRADE_SHORTCODE_FORMAT = "{U0}";
        private const string DYNAMIC_VALUE_SHORTCODE_FORMAT = "{V0}";
        private const string PARAMETER_SHORTCODE_FORMAT = "{P0}";

        private static readonly Color MentionedUpgradeColor = new(0.7f, 0.7f, 0.7f);

        [SerializeField, TextArea(4, 16)] private string upgradeDescription = "Description";
        [Space]
        [SerializeField] private Mentions mentions;

        public string FormatDescription(UTUpgradeSO upgradeSO)
        {
            string description = upgradeDescription;

            for (int i = 0; i < mentions.MentionedCurrencies.Count; i++)
            {
                UTCurrencySO currency = mentions.MentionedCurrencies[i];
                if (currency == null) continue;
                string replace = CURRENCY_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, currency.ToString());
            }

            for (int i = 0; i < mentions.MentionedUpgrades.Count; i++)
            {
                UTUpgradeSO upgrade = mentions.MentionedUpgrades[i];
                if (upgrade == null) continue;
                string replace = UPGRADE_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, upgrade.ToString().WrapInColor(MentionedUpgradeColor));
            }

            for (int i = 0; i < mentions.MentionedDynamicValues.Count; i++)
            {
                ADynamicValueReferenceSO reference = mentions.MentionedDynamicValues[i];
                if (reference == null) continue;
                string replace = DYNAMIC_VALUE_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, reference.GetValueString());
            }

            for (int i = 0; i < upgradeSO.Parameters.Parameters.Count; i++)
            {
                AUpgradeParameter parameter = upgradeSO.Parameters.Parameters[i];
                string replace = PARAMETER_SHORTCODE_FORMAT.Replace("0", i.ToString());
                description = description.Replace(replace, parameter.GetInfoText());
            }

            return description;
        }
    }
}