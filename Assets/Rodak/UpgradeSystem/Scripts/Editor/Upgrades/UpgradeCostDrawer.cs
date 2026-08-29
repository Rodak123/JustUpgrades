using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    [CustomPropertyDrawer(typeof(UpgradeCost))]
    public class UpgradeCostDrawer : PropertyDrawer
    {
        public const string TargetCurrencyPropertyName = "targetCurrency";
        public const string CostScalingPropertyName = "costScaling";
        public const string IsSpentPropertyName = "isSpent";

        private SerializedProperty targetCurrencyProperty;
        private SerializedProperty costScalingProperty;
        private SerializedProperty isSpentProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            targetCurrencyProperty = property.FindPropertyRelative(TargetCurrencyPropertyName);
            costScalingProperty = property.FindPropertyRelative(CostScalingPropertyName);
            isSpentProperty = property.FindPropertyRelative(IsSpentPropertyName);

            if (
                targetCurrencyProperty == null
                || costScalingProperty == null
                || isSpentProperty == null
            )
            {
                return root;
            }

            Foldout foldout = new();
            foldout.BindProperty(property);
            root.Add(foldout);

            PropertyField targetUpgradeField = new(targetCurrencyProperty);
            targetUpgradeField.Bind(targetCurrencyProperty.serializedObject);
            foldout.Add(targetUpgradeField);

            PropertyField costScalingField = new(costScalingProperty);
            costScalingField.Bind(costScalingProperty.serializedObject);
            foldout.Add(costScalingField);

            PropertyField isSpentField = new(isSpentProperty);
            isSpentField.Bind(isSpentProperty.serializedObject);
            foldout.Add(isSpentField);

            void UpdateUI()
            {
                UpdateFoldoutText(foldout, property);
            }

            UpdateUI();

            root.TrackPropertyValue(targetCurrencyProperty, (_) => UpdateUI());
            root.TrackPropertyValue(costScalingProperty, (_) => UpdateUI());

            return root;
        }

        private ACurrencySO GetTargetCurrency()
        {
            if (targetCurrencyProperty == null) return null;
            return (ACurrencySO)targetCurrencyProperty.objectReferenceValue;
        }

        private void UpdateFoldoutText(Foldout foldout, SerializedProperty property)
        {
            ACurrencySO targetCurrency = GetTargetCurrency();
            string foldoutName = property.displayName;

            if (targetCurrency == null)
            {
                foldoutName += $" ( Target Currency not set )";
            }
            else
            {
                foldoutName += $" ( {CurrencyPresenter.GetFormattedCurrency(targetCurrency)} )";
            }

            foldout.text = foldoutName;
        }
    }
}