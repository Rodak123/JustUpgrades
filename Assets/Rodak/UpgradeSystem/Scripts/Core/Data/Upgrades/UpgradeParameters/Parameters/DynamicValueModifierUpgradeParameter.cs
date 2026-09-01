using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable, CreateUpgradeParameterMenu(Order = 100, MenuName = "Dynamic Value Modifier")]
    public class DynamicValueModifierUpgradeParameter : AUpgradeParameter, IHasDynamicValues
    {
        private class Modifier : IDynamicValueModifier
        {
            private readonly DynamicValueModifierUpgradeParameter upgradeParameter;

            private BigDouble value;

            public Modifier(DynamicValueModifierUpgradeParameter upgradeParameter)
            {
                this.upgradeParameter = upgradeParameter;
            }

            public void Query(DynamicValueQuery query)
            {
                query.Value += value;
            }

            public void Update()
            {
                value = upgradeParameter.DynamicValueModifier.GetValue(upgradeParameter.CurrentLevel, upgradeParameter.MaxLevel);
            }
        }

        [Tooltip("The target dynamic value that is modified")]
        [SerializeField] private ADynamicValueReferenceSO targetDynamicValue;

        [Tooltip("The way the target value is modified")]
        [SerializeField] private ScaledValue dynamicValueModifier;

        [Header("Visual")]
        [SerializeField] private bool showInfoText;

        private Upgrade upgrade;
        private Modifier modifier;

        private int CurrentLevel => upgrade.CurrentLevel;
        private int MaxLevel => upgrade.MaxLevel;

        public ScaledValue DynamicValueModifier => dynamicValueModifier;

        public override void Init(AUpgradeSO upgradeSO)
        {
            if (targetDynamicValue == null)
            {
                Debug.LogError($"Can't initialize parameter '{nameof(DynamicValueModifierUpgradeParameter)}': {nameof(targetDynamicValue)} is null. In: {upgradeSO}");
                return;
            }

            UpgradeSystemContext.Instance.UpgradeTracker.TryGetUpgrade(upgradeSO, out upgrade);

            if (upgrade == null)
            {
                Debug.LogError($"Can't initialize parameter '{nameof(DynamicValueModifierUpgradeParameter)}': {nameof(upgradeSO)} '{upgradeSO}' is not registered. In: {upgradeSO}");
                return;
            }

            modifier = new Modifier(this);

            UpgradeSystemContext.Instance.DynamicValueRegistry.AttachModifier(targetDynamicValue, modifier);
        }

        public override void Update()
        {
            if (modifier == null || upgrade == null)
            {
                return;
            }

            modifier.Update();
        }

        public override string GetInfoText()
        {
            if (modifier == null)
            {
                return "|Uninitialized|";
            }

            if (!showInfoText)
            {
                return string.Empty;
            }

            BigDouble value = DynamicValueModifier.GetValue(upgrade.CurrentLevel, MaxLevel);
            return DynamicValuePresenter.GetFormattedDynamicValue(value);
        }

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            return dynamicValueModifier.GetDynamicValues();
        }

        public override string GetEditorDescription()
        {
            if (targetDynamicValue == null) return $"Missing '{nameof(targetDynamicValue)}'!";
            return $"Modifies '{targetDynamicValue}'";
        }
    }
}