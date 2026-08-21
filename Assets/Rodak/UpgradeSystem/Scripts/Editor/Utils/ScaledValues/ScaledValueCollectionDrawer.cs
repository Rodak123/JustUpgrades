using Rodak.UpgradeSystem.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(ScaledValueCollection))]
    public class ScaledValueCollectionDrawer : PropertyDrawer
    {
        public const string ScaledValuesPropertyName = "scaledValues";

        private const string UnityAddButtonName = "unity-list-view__add-button";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            SerializedProperty scaledValuesProperty = property.FindPropertyRelative(ScaledValuesPropertyName);

            if (scaledValuesProperty == null)
            {
                return root;
            }

            ListView listView = new();
            listView.BindProperty(scaledValuesProperty);
            root.Add(listView);

            listView.headerTitle = property.displayName;
            listView.showFoldoutHeader = true;
            listView.showAddRemoveFooter = true;
            listView.showBorder = true;
            listView.reorderable = true;
            listView.reorderMode = ListViewReorderMode.Animated;
            listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            listView.style.flexGrow = 1;

            listView.makeItem = () =>
            {
                PropertyField field = new();
                field.style.flexGrow = 1;
                field.style.flexBasis = 0;
                return field;
            };

            listView.bindItem = (VisualElement element, int index) =>
            {
                PropertyField field = (PropertyField)element;
                field.BindProperty(scaledValuesProperty.GetArrayElementAtIndex(index));
            };

            listView.schedule.Execute(() =>
            {
                Button builtInAddButton = listView.Q<Button>(UnityAddButtonName);

                if (builtInAddButton != null)
                {
                    builtInAddButton.clickable.activators.Clear();

                    builtInAddButton.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        if (evt.button != 0) return;

                        int index = scaledValuesProperty.arraySize;
                        scaledValuesProperty.arraySize++;

                        SerializedProperty newElement = scaledValuesProperty.GetArrayElementAtIndex(index);
                        SerializedProperty scalerFieldProperty = newElement.FindPropertyRelative(ScaledValueDrawer.ScalerPropertyName);

                        if (scalerFieldProperty != null)
                        {
                            scalerFieldProperty.managedReferenceValue = null;
                        }


                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
            });

            return root;
        }
    }
}