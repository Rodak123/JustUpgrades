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
    [CustomPropertyDrawer(typeof(UpgradeRequirements))]
    public class UpgradeRequirementsDrawer : PropertyDrawer
    {
        private const string NameShortcodeFormat = "{NAME}";

        public const string RequirementsPropertyName = "requirements";

        private SerializedProperty requirementsProperty;

        private void UpdateRequirementLabel(PropertyField field, SerializedProperty property)
        {
            if (property == null || property.serializedObject == null)
                return;

            AUpgradeRequirement requirement = property.managedReferenceValue as AUpgradeRequirement;

            string description = requirement != null ? requirement.GetEditorDescription() : "None";
            string label = $"{property.displayName} ( {description} )";

            field.label = label;

            Foldout foldout = field.Q<Foldout>();
            if (foldout != null)
            {
                foldout.text = label;
            }
        }

        private VisualElement CreateRequirementsList()
        {
            ListView requirementsList = new()
            {
                bindingPath = requirementsProperty.propertyPath,
                makeItem = () =>
                {
                    PropertyField field = new();

                    // for now disabled, since enabling cancels focus after each update
                    // field.RegisterCallback<SerializedPropertyChangeEvent>(evt =>
                    // {
                    //     if (field.userData is SerializedProperty property)
                    //     {
                    //         field.schedule.Execute(() => UpdateRequirementLabel(field, property));
                    //     }
                    // });

                    return field;
                },
                bindItem = (element, index) =>
                {
                    SerializedProperty indexProperty = requirementsProperty.GetArrayElementAtIndex(index);
                    PropertyField field = (PropertyField)element;

                    field.userData = indexProperty;
                    field.BindProperty(indexProperty);

                    field.TrackPropertyValue(indexProperty, prop =>
                    {
                        UpdateRequirementLabel(field, prop);
                    });

                    field.schedule.Execute(() =>
                    {
                        UpdateRequirementLabel(field, indexProperty);
                    });
                },
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                showAddRemoveFooter = true,
                showFoldoutHeader = true,
                showBorder = true,
                headerTitle = "Requirements",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                showBoundCollectionSize = false
            };

            requirementsList.schedule.Execute(() =>
            {
                Button builtInAddButton = requirementsList.Q<Button>(EditorUtils.UnityListViewAddButtonName);

                if (builtInAddButton != null)
                {
                    builtInAddButton.clickable.activators.Clear();

                    builtInAddButton.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        if (evt.button != 0) return;

                        GenericMenu menu = GenerateTypeSelectionMenu(GetRequirementTypes());
                        menu.ShowAsContext();
                    });
                }
            });

            return requirementsList;
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
                CreateUpgradeRequirementMenuAttribute attribute = type.GetAttribute<CreateUpgradeRequirementMenuAttribute>();
                if (attribute == null || !attribute.ShowRequirement)
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
                menu.AddItem(new GUIContent(item.Name), false, OnAddRequirement, item.Type);
            }

            return menu;
        }

        private IEnumerable<Type> GetRequirementTypes()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<AUpgradeRequirement>();

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

        private void OnAddRequirement(object targetType)
        {
            Type type = (Type)targetType;
            int index = requirementsProperty.arraySize;
            requirementsProperty.arraySize++;

            SerializedProperty element = requirementsProperty.GetArrayElementAtIndex(index);

            element.managedReferenceValue = Activator.CreateInstance(type);

            requirementsProperty.serializedObject.ApplyModifiedProperties();
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            requirementsProperty = property.FindPropertyRelative(RequirementsPropertyName);

            if (
                requirementsProperty == null
            )
            {
                Debug.LogWarning("No all properties found.");
                return root;
            }

            VisualElement requirementsList = CreateRequirementsList();
            root.Add(requirementsList);

            return root;
        }
    }
}