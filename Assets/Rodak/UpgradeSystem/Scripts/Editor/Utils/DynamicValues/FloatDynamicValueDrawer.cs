using Rodak.UpgradeSystem.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(FloatDynamicValue))]
    public class FloatDynamicValueDrawer : ADynamicValueDrawer<FloatDynamicValueReferenceSO>
    {
        public const string BaseValuePropertyName = "baseValue";

        protected override VisualElement CreateValueGUI(SerializedProperty property)
        {
            SerializedProperty baseValueProperty = property.FindPropertyRelative(BaseValuePropertyName);

            if (baseValueProperty == null)
            {
                return null;
            }

            PropertyField baseValueField = new(baseValueProperty)
            {
                label = EditorUtils.GetPreferredDisplayLabel(this, property)
            };

            return baseValueField;
        }
    }
}