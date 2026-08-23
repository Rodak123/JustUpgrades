using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    public static class EditorUtils
    {
        public const string UnityScriptPropertyName = "m_Script";
        public const string UnityContentContainerName = "unity-content";

        public static void AppendOtherProperties(this SerializedObject serializedObject, VisualElement root, IEnumerable<string> ignoredPropertiesList = null)
        {
            SerializedProperty iterator = serializedObject.GetIterator();

            iterator.NextVisible(true);

            HashSet<string> ignoredProperties = ignoredPropertiesList == null ? new() : new(ignoredPropertiesList);
            ignoredProperties.Add(UnityScriptPropertyName);

            do
            {
                if (ignoredProperties.Contains(iterator.name))
                    continue;

                PropertyField field = new(iterator.Copy());
                field.Bind(serializedObject);
                root.Add(field);
            }
            while (iterator.NextVisible(false));
        }

        public static string GetFullDisplayPath(this SerializedProperty property)
        {
            if (property == null)
                return string.Empty;

            return property.propertyPath;
        }

        public static string GetPreferredDisplayLabel(PropertyDrawer propertyDrawer, SerializedProperty property)
        {
            return !string.IsNullOrEmpty(propertyDrawer.preferredLabel) ? propertyDrawer.preferredLabel : property.displayName;
        }
    }
}