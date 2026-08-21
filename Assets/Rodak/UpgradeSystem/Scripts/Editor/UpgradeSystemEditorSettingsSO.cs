using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CreateAssetMenu(fileName = "UpgradeSystemEditorSettings", menuName = "UpgradeSystem/Dev/Editor Settings", order = 10000)]
    public class UpgradeSystemEditorSettingsSO : ScriptableObject
    {
        [Header("Changing Value")]
        [SerializeField] private VisualTreeAsset changingValueDocument;

        [Header("Currency")]
        [SerializeField] private VisualTreeAsset currencyDocument;

        [Header("Upgrade")]
        [SerializeField] private VisualTreeAsset aUpgradeDocument;
        [SerializeField] private VisualTreeAsset upgradeRequirementDocument;
        [SerializeField] private VisualTreeAsset upgradeCostDocument;

        [Header("Parameter Upgrade")]
        [SerializeField] private VisualTreeAsset parameterUpgradeDocument;
        [SerializeField] private VisualTreeAsset aUpgradeParameterDocument;

        [Header("Upgrade System Context")]
        [SerializeField] private VisualTreeAsset upgradeSystemContextDocument;

        public VisualTreeAsset CurrencyDocument => currencyDocument;

        public VisualTreeAsset AUpgradeDocument => aUpgradeDocument;
        public VisualTreeAsset UpgradeRequirementDocument => upgradeRequirementDocument;
        public VisualTreeAsset UpgradeCostDocument => upgradeCostDocument;

        public VisualTreeAsset ParameterUpgradeDocument => parameterUpgradeDocument;
        public VisualTreeAsset AUpgradeParameterDocument => aUpgradeParameterDocument;

        public VisualTreeAsset UpgradeSystemContextDocument => upgradeSystemContextDocument;

        private static UpgradeSystemEditorSettingsSO instance;

        public static UpgradeSystemEditorSettingsSO Instance
        {
            get
            {
                if (instance == null)
                {
                    string[] guids = AssetDatabase.FindAssets($"t:{nameof(UpgradeSystemEditorSettingsSO)}");

                    if (guids.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<UpgradeSystemEditorSettingsSO>(path);
                    }
                    else
                    {
                        Debug.LogError($"No {nameof(UpgradeSystemEditorSettingsSO)} found.");
                    }
                }

                return instance;
            }
        }
    }
}