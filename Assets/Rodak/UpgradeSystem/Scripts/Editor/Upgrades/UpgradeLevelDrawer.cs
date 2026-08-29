using Rodak.UpgradeSystem.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(UpgradeLevel))]
    public class UpgradeLevelDrawer : PropertyDrawer
    {
        public const string MaxLevelPropertyName = "maxLevel";
        public const string IsInfinitePropertyName = "isInfinite";

        private SerializedProperty maxLevelProperty;
        private SerializedProperty isInfiniteProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            maxLevelProperty = property.FindPropertyRelative(MaxLevelPropertyName);
            isInfiniteProperty = property.FindPropertyRelative(IsInfinitePropertyName);

            if (
                maxLevelProperty == null
                || isInfiniteProperty == null
            )
            {
                return root;
            }

            Foldout foldout = new();
            foldout.text = property.displayName;
            foldout.BindProperty(property);
            root.Add(foldout);

            PropertyField maxLevelField = new(maxLevelProperty);
            maxLevelField.Bind(maxLevelProperty.serializedObject);
            foldout.Add(maxLevelField);

            PropertyField isInfiniteField = new(isInfiniteProperty);
            isInfiniteField.Bind(isInfiniteProperty.serializedObject);
            foldout.Add(isInfiniteField);

            return root;
        }
    }
}