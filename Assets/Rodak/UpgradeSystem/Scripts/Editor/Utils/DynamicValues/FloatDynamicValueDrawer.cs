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

            Foldout foldout = new()
            {
                text = property.displayName,
            };
            foldout.BindProperty(property);

            root.Add(foldout);

            VisualElement contentContainer = foldout.Q<VisualElement>(EditorUtils.UnityContentContainerName);

            PropertyField baseAmountField = new(baseAmountProperty);
            contentContainer.Add(baseAmountField);

            AttachExposeButton(contentContainer, property);

            PropertyField exposedReferenceField = new(exposedReferenceProperty)
            {
                enabledSelf = false
            };
            contentContainer.Add(exposedReferenceField);

            // this stores the previous value to the userData cache so that it can remove it from assets when its cleared.
            exposedReferenceField.RegisterValueChangeCallback((SerializedPropertyChangeEvent evt) =>
            {
                FloatDynamicValueReferenceSO newSO = evt.changedProperty.objectReferenceValue as FloatDynamicValueReferenceSO;

                if (newSO != null)
                {
                    exposedReferenceField.userData = newSO;
                    return;
                }

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

            return root;
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
                button.text = isExposed ? "Hide" : "Expose";
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

                Debug.Log(property.GetFullDisplayPath());

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