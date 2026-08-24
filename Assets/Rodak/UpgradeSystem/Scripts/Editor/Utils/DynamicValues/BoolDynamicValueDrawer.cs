using Rodak.UpgradeSystem.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(BoolDynamicValue))]
    public class BoolDynamicValueDrawer : ADynamicValueDrawer<BoolDynamicValueReferenceSO>
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