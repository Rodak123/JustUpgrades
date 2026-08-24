using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    public abstract class ADynamicValueDrawer<R> : PropertyDrawer where R : ADynamicValueReferenceSO
    {
        private const string ExposedIconName = "animationvisibilitytoggleon";
        private const string HiddenIconName = "animationvisibilitytoggleoff";

        public const string ExposedReferencePropertyName = "exposedReference";

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
                R newSO = evt.changedProperty.objectReferenceValue as R;

                if (newSO != null)
                {
                    exposedReferenceField.userData = newSO;
                    exposedReferenceField.style.display = DisplayStyle.Flex;
                    return;
                }

                exposedReferenceField.style.display = DisplayStyle.None;

                R oldSO = exposedReferenceField.userData as R;

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

        private string GetReferenceName(SerializedProperty property)
        {
            string[] elements = property.GetFullDisplayPath().Split('.');

            string name = "";
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                name += elements[i];
                if (i > 0) name += '_';
            }

            return name;
        }

        private void AttachExposeButton(VisualElement root, SerializedProperty property, SerializedProperty exposedReferenceProperty)
        {
            Button button = new(ManageExposedValue);
            button.clicked += () => UpdateUI();
            root.Add(button);

            UpdateUI();

            R GetExposedReference()
            {
                return (R)exposedReferenceProperty.objectReferenceValue;
            }

            void UpdateUI()
            {
                bool isExposed = GetExposedReference() != null;
                button.text = "";
                button.iconImage = GetExposeButtonIcon(isExposed);
            }

            void ManageExposedValue()
            {
                bool isExposed = GetExposedReference() != null;
                if (isExposed) UnExposeValue();
                else ExposeValue();
            }

            void UnExposeValue()
            {
                R reference = (R)exposedReferenceProperty.objectReferenceValue;
                if (reference == null) return;

                exposedReferenceProperty.serializedObject.Update();
                exposedReferenceProperty.objectReferenceValue = null;
                exposedReferenceProperty.serializedObject.ApplyModifiedProperties();
            }

            void ExposeValue()
            {
                R oldReference = (R)exposedReferenceProperty.objectReferenceValue;
                if (oldReference != null) return;

                R reference = ScriptableObject.CreateInstance<R>();

                reference.name = GetReferenceName(property);

                Object targetObject = property.serializedObject.targetObject;

                if (targetObject is UpgradeSO parentUpgrade)
                {
                    reference.name = $"{reference.name}_{parentUpgrade.GetChangingValueSuffix()}";

                    AssetDatabase.AddObjectToAsset(reference, parentUpgrade);
                }
                else if (targetObject is ScriptableObject scriptableObject)
                {
                    reference.name = $"{reference.name}_{scriptableObject.name}";

                    AssetDatabase.AddObjectToAsset(reference, scriptableObject);
                }
                else
                {
                    AssetDatabase.CreateAsset(reference, $"Assets/{reference.name}_{targetObject.name}.asset");
                }

                AssetDatabase.SaveAssets();

                exposedReferenceProperty.serializedObject.Update();
                exposedReferenceProperty.objectReferenceValue = reference;
                exposedReferenceProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        protected abstract VisualElement CreateValueGUI(SerializedProperty property);

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            SerializedProperty exposedReferenceProperty = property.FindPropertyRelative(ExposedReferencePropertyName);

            if (exposedReferenceProperty == null)
            {
                return root;
            }

            VisualElement row = new();
            row.style.flexDirection = FlexDirection.Row;
            root.Add(row);

            AttachExposeButton(row, property, exposedReferenceProperty);

            VisualElement valueGUI = CreateValueGUI(property);
            if (valueGUI == null)
            {
                valueGUI = new Label("No Value GUI");
            }

            valueGUI.style.flexGrow = 1;
            row.Add(valueGUI);

            AttachExposedReferenceField(root, exposedReferenceProperty, property);

            return root;
        }
    }
}