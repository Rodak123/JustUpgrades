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
    [CustomPropertyDrawer(typeof(UpgradeParameters))]
    public class UpgradeParametersDrawer : PropertyDrawer
    {
        private const string NameShortcodeFormat = "{NAME}";

        public const string ParametersPropertyName = "parameters";

        private SerializedProperty parametersProperty;

        private void UpdateParameterLabel(PropertyField field, SerializedProperty property)
        {
            if (property == null || property.serializedObject == null)
                return;

            AUpgradeParameter parameter = property.managedReferenceValue as AUpgradeParameter;

            string description = parameter != null ? parameter.GetEditorDescription() : "None";
            string label = $"{property.displayName} ( {description} )";

            field.label = label;

            Foldout foldout = field.Q<Foldout>();
            if (foldout != null)
            {
                foldout.text = label;
            }
        }

        private VisualElement CreateParametersList()
        {
            ListView parametersList = new()
            {
                bindingPath = parametersProperty.propertyPath,
                makeItem = () =>
                {
                    PropertyField field = new();

                    // for now disabled, since enabling cancels focus after each update
                    // field.RegisterCallback<SerializedPropertyChangeEvent>(evt =>
                    // {
                    //     if (field.userData is SerializedProperty property)
                    //     {
                    //         field.schedule.Execute(() => UpdateParameterLabel(field, property));
                    //     }
                    // });

                    return field;
                },
                bindItem = (element, index) =>
                {
                    SerializedProperty indexProperty = parametersProperty.GetArrayElementAtIndex(index);
                    PropertyField field = (PropertyField)element;

                    field.userData = indexProperty;
                    field.BindProperty(indexProperty);

                    field.TrackPropertyValue(indexProperty, prop =>
                    {
                        UpdateParameterLabel(field, prop);
                    });

                    field.schedule.Execute(() =>
                    {
                        UpdateParameterLabel(field, indexProperty);
                    });
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

            parametersProperty.serializedObject.ApplyModifiedProperties();
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            parametersProperty = property.FindPropertyRelative(ParametersPropertyName);

            if (
                parametersProperty == null
            )
            {
                Debug.LogWarning("No all properties found.");
                return root;
            }

            VisualElement parametersList = CreateParametersList();
            root.Add(parametersList);

            return root;
        }
    }
}