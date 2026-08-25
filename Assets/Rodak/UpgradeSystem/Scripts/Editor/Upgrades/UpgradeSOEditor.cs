using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using Rodak.Utils.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomEditor(typeof(UpgradeSO), true)]
    public class UpgradeSOEditor : UnityEditor.Editor
    {
        protected virtual IEnumerable<string> GetIgnoredProperties()
        {
            yield return "";
        }

        protected virtual VisualElement CreateRootGUI()
        {
            VisualElement root = new();
            // VisualTreeAsset document = UpgradeSystemEditorSettingsSO.Instance?.AUpgradeDocument;

            // upgradeNameProperty = serializedObject.FindProperty(UpgradeNamePropertyName);
            // hasMaxLevelProperty = serializedObject.FindProperty(HasMaxLevelPropertyName);
            // maxLevelProperty = serializedObject.FindProperty(MaxLevelPropertyName);
            // costsProperty = serializedObject.FindProperty(CostsPropertyName);
            // requirementsProperty = serializedObject.FindProperty(RequirementsPropertyName);

            // if (document == null
            //     || upgradeNameProperty == null
            //     || hasMaxLevelProperty == null
            //     || maxLevelProperty == null
            //     || costsProperty == null
            //     || requirementsProperty == null
            // )
            // {
            //     return root;
            // }

            // document.CloneTree(root);

            // TextField upgradeNameField = root.Q<TextField>(UpgradeNameFieldName);
            // upgradeNameField.bindingPath = upgradeNameProperty.propertyPath;

            // Toggle hasMaxLevelToggle = root.Q<Toggle>(HasMaxLevelToggleName);
            // hasMaxLevelToggle.bindingPath = hasMaxLevelProperty.propertyPath;

            // IntegerField maxLevelField = root.Q<IntegerField>(MaxLevelFieldName);
            // maxLevelField.bindingPath = maxLevelProperty.propertyPath;

            // ListView costsList = root.Q<ListView>(CostsListName);
            // costsList.bindingPath = costsProperty.propertyPath;

            // ListView requirementsList = root.Q<ListView>(RequirementsListName);
            // requirementsList.bindingPath = requirementsProperty.propertyPath;

            // void UpdateUI()
            // {
            //     if (maxLevelProperty.intValue < 1)
            //     {
            //         maxLevelProperty.intValue = 1;
            //         serializedObject.ApplyModifiedProperties();
            //     }

            //     maxLevelField.SetEnabled(hasMaxLevelProperty.boolValue);
            // }

            // UpdateUI();

            // root.TrackPropertyValue(upgradeNameProperty, (_) => UpdateUI());
            // root.TrackPropertyValue(hasMaxLevelProperty, (_) => UpdateUI());
            // root.TrackPropertyValue(maxLevelProperty, (_) => UpdateUI());
            // root.TrackPropertyValue(costsProperty, (_) => UpdateUI());
            // root.TrackPropertyValue(requirementsProperty, (_) => UpdateUI());

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