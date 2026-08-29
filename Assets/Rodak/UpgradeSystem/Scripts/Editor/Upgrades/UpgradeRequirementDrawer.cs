using Rodak.UpgradeSystem.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(UpgradeRequirement))]
    public class UpgradeRequirementDrawer : PropertyDrawer
    {
        public const string TargetUpgradePropertyName = "targetUpgrade";
        public const string UseMaxLevelPropertyName = "useMaxLevel";
        public const string TargetLevelPropertyName = "targetLevel";

        private SerializedProperty targetUpgradeProperty;
        private SerializedProperty useMaxLevelProperty;
        private SerializedProperty targetLevelProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            targetUpgradeProperty = property.FindPropertyRelative(TargetUpgradePropertyName);
            useMaxLevelProperty = property.FindPropertyRelative(UseMaxLevelPropertyName);
            targetLevelProperty = property.FindPropertyRelative(TargetLevelPropertyName);

            if (
                targetUpgradeProperty == null
                || useMaxLevelProperty == null
                || targetLevelProperty == null
            )
            {
                return root;
            }

            Foldout foldout = new();
            foldout.BindProperty(property);
            root.Add(foldout);

            VisualElement targetUpgradeContainer = new();
            foldout.Add(targetUpgradeContainer);

            PropertyField targetUpgradeField = new(targetUpgradeProperty);
            targetUpgradeField.Bind(targetUpgradeProperty.serializedObject);
            targetUpgradeContainer.Add(targetUpgradeField);

            PropertyField useMaxLevelField = new(useMaxLevelProperty);
            useMaxLevelField.Bind(useMaxLevelProperty.serializedObject);
            foldout.Add(useMaxLevelField);

            PropertyField targetLevelField = new(targetLevelProperty);
            targetLevelField.Bind(targetLevelProperty.serializedObject);
            foldout.Add(targetLevelField);

            void UpdateUI()
            {
                UpdateFoldoutText(foldout, property);
            }

            UpdateUI();

            root.TrackPropertyValue(targetUpgradeProperty, (_) => UpdateUI());
            root.TrackPropertyValue(useMaxLevelProperty, (_) => UpdateUI());
            root.TrackPropertyValue(targetLevelProperty, (_) => UpdateUI());

            return root;
        }

        private AUpgradeSO GetTargetUpgrade()
        {
            if (targetUpgradeProperty == null) return null;
            return (AUpgradeSO)targetUpgradeProperty.objectReferenceValue;
        }

        private void UpdateFoldoutText(Foldout foldout, SerializedProperty property)
        {
            foldout.text = $"{property.displayName} ( {targetUpgradeProperty.objectReferenceValue} )";
        }
    }
}