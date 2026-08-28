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

        public override void Init(UpgradeSystemContext context, AUpgradeSO upgradeSO)
        {
            modifier = new Modifier(this);

            context.UpgradeRegistry.TryGetUpgrade(upgradeSO, out upgrade);
            context.DynamicValueRegistry.AttachModifier(targetDynamicValue, modifier);

            if (upgrade == null)
            {
                throw new ArgumentNullException($"{nameof(upgradeSO)} '{upgradeSO}' is not registered.");
            }
        }

        public override void Update()
        {
            if (modifier == null)
            {
                Debug.LogWarning($"Parameter '{this}' not initialized.");
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

        public override string ToString()
        {
            return $"Modifies '{targetDynamicValue}'";
        }
    }
}