using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using Rodak.Utils.Editor;
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
        private const string RenameReferenceIconName = "CustomTool";

        public const string ExposedReferencePropertyName = "exposedReference";

        private SerializedProperty property;
        private SerializedProperty exposedReferenceProperty;

        private void AttachExposedReferenceField(VisualElement root)
        {
            PropertyField exposedReferenceField = new(exposedReferenceProperty)
            {
                enabledSelf = false
            };
            root.style.display = DisplayStyle.None;
            root.Add(exposedReferenceField);

            // this stores the previous value to the userData cache so that it can remove it from assets when its cleared.
            exposedReferenceField.RegisterValueChangeCallback(evt =>
            {
                R newSO = evt.changedProperty.objectReferenceValue as R;

                if (newSO != null)
                {
                    exposedReferenceField.userData = newSO;
                    root.style.display = DisplayStyle.Flex;
                    return;
                }

                root.style.display = DisplayStyle.None;

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
            return EditorUtils.CreateEditorIcon(iconName);
        }

        private string GetReferenceName()
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

        private void AttachExposeButton(VisualElement root)
        {
            Button button = new(ManageExposedValue);
            button.clicked += () => UpdateUI();
            root.Add(button);

            UpdateUI();

            void UpdateUI()
            {
                R exposedReference = (R)exposedReferenceProperty.objectReferenceValue;

                bool isExposed = exposedReference != null;

                button.text = "";
                button.tooltip = isExposed
                    ? $"Delete reference: {exposedReference}"
                    : "Expose reference for modifiers";
                button.iconImage = GetExposeButtonIcon(isExposed);
            }

            void ManageExposedValue()
            {
                R exposedReference = (R)exposedReferenceProperty.objectReferenceValue;

                if (exposedReference != null) UnExposeValue();
                else ExposeValue();
            }
        }


        private void UnExposeValue()
        {
            R reference = (R)exposedReferenceProperty.objectReferenceValue;
            if (reference == null) return;

            exposedReferenceProperty.serializedObject.Update();
            exposedReferenceProperty.objectReferenceValue = null;
            exposedReferenceProperty.serializedObject.ApplyModifiedProperties();
        }

        private void ExposeValue()
        {
            R oldReference = (R)exposedReferenceProperty.objectReferenceValue;
            if (oldReference != null) return;

            R reference = ScriptableObject.CreateInstance<R>();

            reference.name = GetReferenceName();

            Object targetObject = property.serializedObject.targetObject;

            if (targetObject is AUpgradeSO parentUpgrade)
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

        private void AttachRenameReferenceButton(VisualElement root)
        {
            Button button = new(RenameReference)
            {
                text = "",
                tooltip = $"Rename reference",
                iconImage = EditorUtils.CreateEditorIcon(RenameReferenceIconName),
            };
            root.Add(button);

            void RenameReference()
            {
                R exposedReference = (R)exposedReferenceProperty.objectReferenceValue;

                TextPromptWindow.ShowPrompt(
                    "Pick a new name:",
                    (newName) =>
                    {
                        if (newName.Length == 0)
                        {
                            Debug.Log($"Name is empty, ignoring.");
                            return;
                        }

                        Object targetObject = property.serializedObject.targetObject;

                        if (targetObject is ScriptableObject scriptableObject)
                        {
                            exposedReference.name = newName;
                        }
                        else
                        {
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(exposedReference), newName);
                        }

                        AssetDatabase.SaveAssets();
                    },
                    null,
                    exposedReference.name
                );
            }
        }

        protected abstract VisualElement CreateValueGUI(SerializedProperty property);

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            this.property = property;
            exposedReferenceProperty = property.FindPropertyRelative(ExposedReferencePropertyName);

            if (exposedReferenceProperty == null)
            {
                return root;
            }

            VisualElement toggleAndValueRow = new();
            toggleAndValueRow.style.flexDirection = FlexDirection.Row;
            root.Add(toggleAndValueRow);

            AttachExposeButton(toggleAndValueRow);

            VisualElement valueGUI = CreateValueGUI(property);
            if (valueGUI == null)
            {
                valueGUI = new Label("No Value GUI");
            }

            valueGUI.style.flexGrow = 1;
            toggleAndValueRow.Add(valueGUI);

            VisualElement referenceRow = new();
            referenceRow.style.flexDirection = FlexDirection.Row;
            root.Add(referenceRow);

            AttachRenameReferenceButton(referenceRow);
            AttachExposedReferenceField(referenceRow);

            return root;
        }
    }
}