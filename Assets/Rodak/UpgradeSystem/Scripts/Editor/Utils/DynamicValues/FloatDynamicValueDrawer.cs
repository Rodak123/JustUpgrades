using System.Reflection;
using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(FloatDynamicValue))]
    public class FloatDynamicValueDrawer : PropertyDrawer
    {
        private const string ExposedIconName = "animationvisibilitytoggleon";
        private const string HiddenIconName = "animationvisibilitytoggleoff";

        public const string BaseAmountPropertyName = "baseAmount";
        public const string ExposedReferencePropertyName = "exposedReference";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            SerializedProperty baseAmountProperty = property.FindPropertyRelative(BaseAmountPropertyName);
            SerializedProperty exposedReferenceProperty = property.FindPropertyRelative(ExposedReferencePropertyName);

            if (baseAmountProperty == null || exposedReferenceProperty == null)
            {
                return root;
            }

            VisualElement row = new();
            row.style.flexDirection = FlexDirection.Row;
            root.Add(row);

            AttachExposeButton(row, property);
            AttachBaseAmountField(row, baseAmountProperty, property);

            AttachExposedReferenceField(root, exposedReferenceProperty, property);

            return root;
        }

        private void AttachBaseAmountField(VisualElement root, SerializedProperty baseAmountProperty, SerializedProperty property)
        {
            PropertyField baseAmountField = new(baseAmountProperty)
            {
                label = property.displayName
            };
            baseAmountField.style.flexGrow = 1;
            root.Add(baseAmountField);
        }

        private void AttachExposedReferenceField(VisualElement root, SerializedProperty exposedReferenceProperty, SerializedProperty property)
        {
            PropertyField exposedReferenceField = new(exposedReferenceProperty)
            {
                enabledSelf = false
            };
            exposedReferenceField.style.display = DisplayStyle.None;
            root.Add(exposedReferenceField);

            // this stores the previous value to the userData cache so that it can remove it from assets when its cleared.
            exposedReferenceField.RegisterValueChangeCallback((SerializedPropertyChangeEvent evt) =>
            {
                FloatDynamicValueReferenceSO newSO = evt.changedProperty.objectReferenceValue as FloatDynamicValueReferenceSO;

                if (newSO != null)
                {
                    exposedReferenceField.userData = newSO;
                    exposedReferenceField.style.display = DisplayStyle.Flex;
                    return;
                }

                exposedReferenceField.style.display = DisplayStyle.None;

                FloatDynamicValueReferenceSO oldSO = exposedReferenceField.userData as FloatDynamicValueReferenceSO;

                if (oldSO == null) return;

                Object targetObject = property.serializedObject.targetObject;

                if (targetObject is ScriptableObject scriptableObject)
                {
                    AssetDatabase.RemoveObjectFromAsset(oldSO);
                }
                else
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oldSO));
                }

                AssetDatabase.SaveAssets();
            });
        }

        private Background GetExposeButtonIcon(bool isExposed)
        {
            string iconName = isExposed ? ExposedIconName : HiddenIconName;
            Texture2D texture = EditorGUIUtility.IconContent(iconName).image as Texture2D;
            return Background.FromTexture2D(texture);
        }

        private void AttachExposeButton(VisualElement root, SerializedProperty property)
        {
            MarkDynamicValueAttribute dynamicValueAttribute = fieldInfo.GetCustomAttribute<MarkDynamicValueAttribute>();
            if (dynamicValueAttribute == null) return;

            FloatDynamicValue FloatDynamicValue = (FloatDynamicValue)property.managedReferenceValue;

            Button button = new(ManageExposedValue);
            button.clicked += () => UpdateUI();
            root.Add(button);

            UpdateUI();

            void UpdateUI()
            {
                bool isExposed = FloatDynamicValue.ExposedReference != null;
                button.text = "";
                button.iconImage = GetExposeButtonIcon(isExposed);
            }

            void ManageExposedValue()
            {
                bool isExposed = FloatDynamicValue.ExposedReference != null;
                if (isExposed) UnExposeValue();
                else ExposeValue();
            }

            void UnExposeValue()
            {
                if (FloatDynamicValue.ExposedReference == null) return;

                FloatDynamicValue.RemoveReference();
            }

            void ExposeValue()
            {
                FloatDynamicValueReferenceSO floatValueChanger = FloatDynamicValue.AddReference();
                if (floatValueChanger == null) return;

                floatValueChanger.name = $"{dynamicValueAttribute.ValueName}_{property.GetFullDisplayPath()}";

                Object targetObject = property.serializedObject.targetObject;

                if (targetObject is UpgradeSO parentUpgrade)
                {
                    floatValueChanger.name = $"{floatValueChanger.name}_{parentUpgrade.GetChangingValueSuffix()}";

                    AssetDatabase.AddObjectToAsset(floatValueChanger, parentUpgrade);
                }
                else if (targetObject is ScriptableObject scriptableObject)
                {
                    floatValueChanger.name = $"{floatValueChanger.name}_{scriptableObject.name}";

                    AssetDatabase.AddObjectToAsset(floatValueChanger, scriptableObject);
                }
                else
                {
                    AssetDatabase.CreateAsset(floatValueChanger, $"Assets/{floatValueChanger.name}_{targetObject.name}.asset");
                }

                AssetDatabase.SaveAssets();
            }
        }

        private bool TryGetParentUpgrade(SerializedProperty property, out UpgradeSO upgrade)
        {
            if (property.serializedObject.targetObject is UpgradeSO parentUpgrade)
            {
                upgrade = parentUpgrade;
                return true;
            }

            upgrade = default;
            return false;
        }
    }
}