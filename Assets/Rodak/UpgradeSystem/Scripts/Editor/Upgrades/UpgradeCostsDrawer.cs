using Rodak.UpgradeSystem.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(UpgradeCosts))]
    public class UpgradeCostsDrawer : PropertyDrawer
    {
        public const string CostsPropertyName = "costs";

        private SerializedProperty costsProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            costsProperty = property.FindPropertyRelative(CostsPropertyName);

            if (
                costsProperty == null
            )
            {
                Debug.LogWarning("No all properties found.");
                return root;
            }

            PropertyField costsField = new(costsProperty);
            costsField.Bind(costsProperty.serializedObject);
            root.Add(costsField);

            return root;
        }
    }
}