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
        public const string UpgradeNamePropertyName = "upgradeName";
        public const string LevelPropertyName = "level";
        public const string CostsArrayPropertyName = "costs";
        public const string RequirementsArrayPropertyName = "requirements";
        public const string ParametersPropertyName = "parameters";

        private SerializedProperty upgradeNameProperty;
        private SerializedProperty levelProperty;
        private SerializedProperty costsProperty;
        private SerializedProperty requirementsProperty;
        private SerializedProperty parametersProperty;

        protected virtual IEnumerable<string> GetIgnoredProperties()
        {
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

            if (
                upgradeNameProperty == null
                || levelProperty == null
                || costsProperty == null
                || requirementsProperty == null
                || parametersProperty == null
            )
            {
                Debug.LogWarning("No all properties found.");
                return root;
            }

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