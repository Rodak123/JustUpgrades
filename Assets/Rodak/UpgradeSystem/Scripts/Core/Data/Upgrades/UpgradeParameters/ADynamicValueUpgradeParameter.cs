using System;
using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable, CreateUpgradeParameterMenu(Order = 100)]
    public class ADynamicValueUpgradeParameter : AUpgradeParameter
    {
        private class Modifier : IDynamicValueModifier
        {
            private readonly ADynamicValueUpgradeParameter upgradeParameter;

            private BigDouble value;

            public Modifier(ADynamicValueUpgradeParameter upgradeParameter)
            {
                this.upgradeParameter = upgradeParameter;
            }

            public void Query(DynamicValueQuery query)
            {
                query.Value += value;
            }

            public void Update()
            {
                value = upgradeParameter.DynamicValueModifier.GetValue(upgradeParameter.CurrentLevel, upgradeParameter.MaxLevel); ;
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
            context.DynamicValuesModifiers.AttachModifier(targetDynamicValue, modifier);

            if (upgrade == null)
            {
                throw new ArgumentNullException($"{nameof(upgradeSO)} '{upgradeSO}' is not registered.");
            }
        }

        public override void ApplyParameterUpgrade(int level, UpgradeSystemContext context) { }

        public override void Update()
        {
            modifier.Update();
        }

        public override string GetInfoText(int level)
        {
            if (!DynamicValueModifier.HasValue || !showInfoText)
            {
                return string.Empty;
            }

            BigDouble value = DynamicValueModifier.GetValue(level, MaxLevel);
            return DynamicValuePresenter.GetFormattedDynamicValue(value);
        }
    }
}