using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(ScaledValue))]
    public class ScaledValueDrawer : PropertyDrawer
    {
        public const string ScalerPropertyName = "scaler";

        public const string UnityContentContainerName = "unity-content";

        private const string ValueScalerPrefix = "ValueScaler";
        private const string NoneOption = "None";

        private static readonly List<Type> ScalerTypeOrder = new()
        {
            typeof(ConstantValueScaler),
            typeof(LinearValueScaler),
            typeof(ExponentialValueScaler),
            typeof(ListValueScaler),
            typeof(GraphValueScaler),
        };

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            SerializedProperty initialScalerProperty = property.FindPropertyRelative(ScalerPropertyName);

            if (initialScalerProperty == null)
            {
                Debug.LogWarning("No all properties found.");
                return root;
            }

            Foldout foldout = new();
            foldout.BindProperty(property);

            root.Add(foldout);

            VisualElement contentContainer = foldout.Q<VisualElement>(UnityContentContainerName);

            DropdownField dropdown = new();
            contentContainer.Add(dropdown);

            VisualElement propertiesContainer = new();
            contentContainer.Add(propertiesContainer);

            List<Type> types = new(GetValueScalerTypes());
            List<string> typeNames = new() { NoneOption };

            foreach (Type type in types)
            {
                typeNames.Add(GetScalerName(type));
            }

            dropdown.choices = typeNames;

            void UpdateUI(SerializedProperty property)
            {
                SerializedProperty scalerProperty = property.FindPropertyRelative(ScalerPropertyName);

                if (scalerProperty == null)
                {
                    return;
                }

                Type currentType = GetScalerType(scalerProperty);
                dropdown.SetValueWithoutNotify(typeNames[currentType != null ? types.IndexOf(currentType) + 1 : 0]);

                RebuildProperties(propertiesContainer, scalerProperty);
                UpdateFoldoutText(foldout, property, scalerProperty);
            }

            dropdown.RegisterValueChangedCallback(evt =>
            {
                SerializedProperty scalerProperty = property.FindPropertyRelative(ScalerPropertyName);

                if (scalerProperty == null)
                {
                    return;
                }

                int index = typeNames.IndexOf(evt.newValue);
                Type newType = index > 0 ? types[index - 1] : null;

                AssignNewScaler(scalerProperty, newType);
                UpdateUI(property);
            });

            root.TrackPropertyValue(property, UpdateUI);
            UpdateUI(property);

            return root;
        }

        private void RebuildProperties(VisualElement propertiesContainer, SerializedProperty scalerProperty)
        {
            propertiesContainer.Clear();

            if (scalerProperty.managedReferenceValue == null)
            {
                return;
            }

            IValueScaler valueScaler = (IValueScaler)scalerProperty.managedReferenceValue;

            Label hintLabel = new Label(valueScaler.Hint());
            propertiesContainer.Add(hintLabel);

            SerializedProperty iterator = scalerProperty.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();

            if (iterator.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(iterator, endProperty))
                    {
                        break;
                    }

                    PropertyField field = new(iterator.Copy());
                    field.BindProperty(iterator);
                    propertiesContainer.Add(field);

                } while (iterator.NextVisible(false));
            }
        }

        private void UpdateFoldoutText(Foldout foldout, SerializedProperty property, SerializedProperty scalerProperty)
        {
            Type currentType = GetScalerType(scalerProperty);
            foldout.text = $"{property.displayName} ({GetScalerName(currentType)})";
        }

        private IEnumerable<Type> GetValueScalerTypes()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<IValueScaler>();
            List<Type> scalerTypes = new();

            foreach (Type type in types)
            {
                if (type.IsAbstract || !type.IsClass || !type.IsSerializable)
                {
                    continue;
                }

                scalerTypes.Add(type);
            }

            scalerTypes.Sort((a, b) =>
            {
                int aIndex = ScalerTypeOrder.IndexOf(a);
                int bIndex = ScalerTypeOrder.IndexOf(b);

                if (aIndex == -1 && bIndex == -1) return 0;
                if (aIndex == -1) return 1;
                if (bIndex == -1) return -1;

                return aIndex.CompareTo(bIndex);
            });

            return scalerTypes;
        }

        private string GetScalerName(Type type)
        {
            if (type == null) return NoneOption;
            return type.Name.Replace(ValueScalerPrefix, string.Empty);
        }

        private Type GetScalerType(SerializedProperty property)
        {
            if (property.managedReferenceValue == null)
            {
                return null;
            }

            foreach (Type type in GetValueScalerTypes())
            {
                if (property.managedReferenceValue.GetType() != type) continue;
                return type;
            }

            return null;
        }

        private void AssignNewScaler(SerializedProperty property, Type newScalerType)
        {
            IValueScaler newScaler = newScalerType != null ? (IValueScaler)Activator.CreateInstance(newScalerType) : null;

            property.serializedObject.Update();
            property.managedReferenceValue = newScaler;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}