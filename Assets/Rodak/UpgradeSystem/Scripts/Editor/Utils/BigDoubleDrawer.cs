#if UNITY_EDITOR
using UnityEditor;
using Rodak.UpgradeSystem.Utils;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using Rodak.Utils.Editor;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(BigDouble))]
    public class BigDoubleDrawer : PropertyDrawer
    {
        public const string MantissaPropertyName = "mantissa";
        public const string ExponentPropertyName = "exponent";

        private static Dictionary<string, int> displayModes = new();

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty mantissaProp = property.FindPropertyRelative(MantissaPropertyName);
            SerializedProperty exponentProp = property.FindPropertyRelative(ExponentPropertyName);

            if (mantissaProp == null || exponentProp == null)
            {
                return new PropertyField(property);
            }

            VisualElement root = new();
            root.style.flexDirection = FlexDirection.Row;

            Label label = new(EditorUtils.GetPreferredDisplayLabel(this, property));
            label.AddToClassList(PropertyField.labelUssClassName);
            label.style.minWidth = 120;
            root.Add(label);

            VisualElement contentContainer = new();
            contentContainer.style.flexDirection = FlexDirection.Row;
            contentContainer.style.flexGrow = 1;
            root.Add(contentContainer);

            DoubleField simpleField = new("D");
            simpleField.style.flexGrow = 1;
            SetupField(simpleField);

            VisualElement scientificContainer = new();
            scientificContainer.style.flexDirection = FlexDirection.Row;
            scientificContainer.style.flexGrow = 1;

            DoubleField mField = new("M");
            mField.BindProperty(mantissaProp);
            SetupField(mField);

            LongField eField = new("E");
            eField.BindProperty(exponentProp);
            SetupField(eField);

            scientificContainer.Add(mField);
            scientificContainer.Add(eField);

            contentContainer.Add(simpleField);
            contentContainer.Add(scientificContainer);

            Button modeBtn = new();
            modeBtn.style.width = 25;
            contentContainer.Add(modeBtn);

            System.Action refreshUI = () =>
            {
                int mode = GetMode(property);
                simpleField.style.display = mode == 1 ? DisplayStyle.Flex : DisplayStyle.None;
                scientificContainer.style.display = mode == 2 ? DisplayStyle.Flex : DisplayStyle.None;
                modeBtn.text = mode.ToString();

                if (mode == 1)
                {
                    BigDouble val = new BigDouble(mantissaProp.doubleValue, exponentProp.longValue);
                    simpleField.SetValueWithoutNotify(val.ToDouble());
                }
            };

            modeBtn.clicked += () =>
            {
                displayModes[property.propertyPath] = GetMode(property) == 1 ? 2 : 1;
                refreshUI();
            };

            mField.RegisterValueChangedCallback(evt => { Normalize(property); refreshUI(); });
            eField.RegisterValueChangedCallback(evt => { Normalize(property); refreshUI(); });

            simpleField.RegisterValueChangedCallback(evt =>
            {
                BigDouble converted = (BigDouble)evt.newValue;
                mantissaProp.doubleValue = converted.Mantissa;
                exponentProp.longValue = converted.Exponent;
                property.serializedObject.ApplyModifiedProperties();
            });

            refreshUI();
            return root;
        }

        private void SetupField(VisualElement field)
        {
            field.style.flexGrow = 1;
            field.style.flexBasis = 0;
            field.style.minWidth = 40;
            if (field is BindableElement be && be.childCount > 0 && be.ElementAt(0) is Label l)
            {
                l.style.minWidth = 15;
            }
        }

        private int GetMode(SerializedProperty property)
        {
            if (displayModes.TryGetValue(property.propertyPath, out int mode)) return mode;
            return 1;
        }

        private void Normalize(SerializedProperty property)
        {
            SerializedProperty m = property.FindPropertyRelative(MantissaPropertyName);
            SerializedProperty e = property.FindPropertyRelative(ExponentPropertyName);
            BigDouble n = BigDouble.Normalize(m.doubleValue, e.longValue);
            m.doubleValue = n.Mantissa;
            e.longValue = n.Exponent;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif