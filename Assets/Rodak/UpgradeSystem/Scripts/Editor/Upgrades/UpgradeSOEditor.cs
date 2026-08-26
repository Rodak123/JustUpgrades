using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using Rodak.Utils.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomEditor(typeof(AUpgradeSO), true)]
    public class UpgradeSOEditor : UnityEditor.Editor
    {
        private const string NameShortcodeFormat = "{NAME}";

        public const string UpgradeNamePropertyName = "upgradeName";
        public const string LevelPropertyName = "level";
        public const string CostsArrayPropertyName = "costs";
        public const string RequirementsArrayPropertyName = "requirements";
        public const string ParametersPropertyName = "parameters";

        private SerializedProperty upgradeNameProperty;
        private SerializedProperty levelProperty;
        private SerializedProperty costsProperty;
        private SerializedProperty requirementsProperty;
        private SerializedProperty parametersProperty;

        private VisualElement CreateParametersList()
        {
            ListView parametersList = new()
            {
                bindingPath = parametersProperty.propertyPath,
                makeItem = () => new PropertyField(),
                bindItem = (element, index) =>
                {
                    PropertyField field = (PropertyField)element;
                    field.BindProperty(parametersProperty.GetArrayElementAtIndex(index));
                },
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                showAddRemoveFooter = true,
                showFoldoutHeader = true,
                showBorder = true,
                headerTitle = "Parameters",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                showBoundCollectionSize = false
            };

            parametersList.schedule.Execute(() =>
            {
                Button builtInAddButton = parametersList.Q<Button>(EditorUtils.UnityListViewAddButtonName);

                if (builtInAddButton != null)
                {
                    builtInAddButton.clickable.activators.Clear();

                    builtInAddButton.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        if (evt.button != 0) return;

                        GenericMenu menu = GenerateTypeSelectionMenu(GetParameterTypes());
                        menu.ShowAsContext();
                    });
                }
            });

            return parametersList;
        }

        private struct TypeMenuItem
        {
            public Type Type;
            public int Order;
            public string Name;
        }

        private GenericMenu GenerateTypeSelectionMenu(IEnumerable<Type> types)
        {
            List<TypeMenuItem> items = new();
            foreach (Type type in types)
            {
                CreateUpgradeParameterMenuAttribute attribute = type.GetAttribute<CreateUpgradeParameterMenuAttribute>();
                if (attribute == null || !attribute.ShowParameter)
                {
                    continue;
                }

                string typeName = GetName(type);

                string name = attribute.MenuName?.Length > 0 ? attribute.MenuName : typeName;
                name = name.Replace(NameShortcodeFormat, typeName);

                items.Add(new()
                {
                    Type = type,
                    Name = name,
                    Order = attribute.Order,
                });
            }

            items.Sort((a, b) => a.Order.CompareTo(b.Order));

            for (int i = 0; i < items.Count; i++)
            {
                TypeMenuItem itemA = items[i];
                bool isUnique = true;

                for (int j = 0; j < items.Count; j++)
                {
                    if (i == j) continue;
                    TypeMenuItem itemB = items[j];

                    if (itemA.Name == itemB.Name)
                    {
                        isUnique = false;

                        itemB.Name += $"/{itemB.Type.FullName}";
                        items[j] = itemB;
                    }
                }

                if (!isUnique)
                {
                    itemA.Name += $"/{itemA.Type.FullName}";
                    items[i] = itemA;
                }
            }

            GenericMenu menu = new();

            foreach (TypeMenuItem item in items)
            {
                menu.AddItem(new GUIContent(item.Name), false, OnAddParameter, item.Type);
            }

            return menu;
        }

        private IEnumerable<Type> GetParameterTypes()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<AUpgradeParameter>();

            foreach (Type type in types)
            {
                if (type.IsAbstract || !type.IsClass || !type.IsSerializable)
                {
                    continue;
                }

                yield return type;
            }
        }

        private string GetName(Type type)
        {
            return ObjectNames.NicifyVariableName(type.Name);
        }

        private void OnAddParameter(object targetType)
        {
            Type type = (Type)targetType;
            int index = parametersProperty.arraySize;
            parametersProperty.arraySize++;

            SerializedProperty element = parametersProperty.GetArrayElementAtIndex(index);

            element.managedReferenceValue = Activator.CreateInstance(type);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual IEnumerable<string> GetIgnoredProperties()
        {
            yield return UpgradeNamePropertyName;
            yield return LevelPropertyName;
            yield return CostsArrayPropertyName;
            yield return RequirementsArrayPropertyName;
            yield return ParametersPropertyName;
        }

        protected virtual VisualElement CreateRootGUI()
        {
            VisualElement root = new();

            upgradeNameProperty = serializedObject.FindProperty(UpgradeNamePropertyName);
            levelProperty = serializedObject.FindProperty(LevelPropertyName);
            costsProperty = serializedObject.FindProperty(CostsArrayPropertyName);
            requirementsProperty = serializedObject.FindProperty(RequirementsArrayPropertyName);
            parametersProperty = serializedObject.FindProperty(ParametersPropertyName);

            if (
                upgradeNameProperty == null
                || levelProperty == null
                || costsProperty == null
                || requirementsProperty == null
                || parametersProperty == null
            )
            {
                Debug.LogWarning("No all properties found.");
                return root;
            }

            PropertyField upgradeNameField = new(upgradeNameProperty);
            root.Add(upgradeNameField);

            PropertyField levelField = new(levelProperty);
            root.Add(levelField);

            PropertyField costsField = new(costsProperty);
            root.Add(costsField);

            PropertyField requirementsField = new(requirementsProperty);
            root.Add(requirementsField);

            VisualElement parametersList = CreateParametersList();
            root.Add(parametersList);

            return root;
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = CreateRootGUI();

            serializedObject.AppendOtherProperties(root, GetIgnoredProperties());

            return root;
        }
    }
}