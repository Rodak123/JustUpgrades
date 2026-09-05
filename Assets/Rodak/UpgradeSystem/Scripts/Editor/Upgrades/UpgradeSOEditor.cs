using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using Rodak.Utils.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomEditor(typeof(AUpgradeSO), true)]
    public class UpgradeSOEditor : UnityEditor.Editor
    {
        private const string RegenerateSaveIdIconName = "d_Refresh";

        public const string SaveIdPropertyName = "saveId";
        public const string UpgradeNamePropertyName = "upgradeName";
        public const string LevelPropertyName = "level";
        public const string CostsArrayPropertyName = "costs";
        public const string RequirementsArrayPropertyName = "requirements";
        public const string ParametersPropertyName = "parameters";

        private SerializedProperty saveIdProperty;
        private SerializedProperty upgradeNameProperty;
        private SerializedProperty levelProperty;
        private SerializedProperty costsProperty;
        private SerializedProperty requirementsProperty;
        private SerializedProperty parametersProperty;

        private void GenerateSaveIdFromName()
        {
            string upgradeName = upgradeNameProperty.stringValue;
            if (string.IsNullOrEmpty(upgradeName))
            {
                Debug.LogWarning("Can't generate from empty name.");
            }
            else
            {
                saveIdProperty.stringValue = UpgradeSystemSaveSystem.SanitizeName(upgradeName);
                serializedObject.ApplyModifiedProperties();
            }
        }

        protected virtual IEnumerable<string> GetIgnoredProperties()
        {
            yield return SaveIdPropertyName;
            yield return UpgradeNamePropertyName;
            yield return LevelPropertyName;
            yield return CostsArrayPropertyName;
            yield return RequirementsArrayPropertyName;
            yield return ParametersPropertyName;
        }

        protected virtual VisualElement CreateRootGUI()
        {
            VisualElement root = new();

            upgradeNameProperty = serializedObject.FindProperty(UpgradeNamePropertyName);
            levelProperty = serializedObject.FindProperty(LevelPropertyName);
            costsProperty = serializedObject.FindProperty(CostsArrayPropertyName);
            requirementsProperty = serializedObject.FindProperty(RequirementsArrayPropertyName);
            parametersProperty = serializedObject.FindProperty(ParametersPropertyName);
            saveIdProperty = serializedObject.FindProperty(SaveIdPropertyName);

            if (
                upgradeNameProperty == null
                || levelProperty == null
                || costsProperty == null
                || requirementsProperty == null
                || parametersProperty == null
                || saveIdProperty == null
            )
            {
                Debug.LogWarning("No all properties found.");
                return root;
            }

            VisualElement saveIdRow = new();
            saveIdRow.style.flexDirection = FlexDirection.Row;
            root.Add(saveIdRow);

            Button generateSaveIdButton = new()
            {
                iconImage = EditorUtils.CreateEditorIcon(RegenerateSaveIdIconName),
                tooltip = "Generate from name"
            };
            generateSaveIdButton.clicked += GenerateSaveIdFromName;
            saveIdRow.Add(generateSaveIdButton);

            PropertyField saveIdField = new(saveIdProperty);
            saveIdField.style.flexGrow = 1;
            saveIdRow.Add(saveIdField);

            PropertyField upgradeNameField = new(upgradeNameProperty);
            root.Add(upgradeNameField);

            PropertyField levelField = new(levelProperty);
            root.Add(levelField);

            PropertyField costsField = new(costsProperty);
            root.Add(costsField);

            PropertyField requirementsField = new(requirementsProperty);
            root.Add(requirementsField);

            PropertyField parametersField = new(parametersProperty);
            root.Add(parametersField);

            return root;
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = CreateRootGUI();

            serializedObject.AppendOtherProperties(root, GetIgnoredProperties());

            return root;
        }
    }
}