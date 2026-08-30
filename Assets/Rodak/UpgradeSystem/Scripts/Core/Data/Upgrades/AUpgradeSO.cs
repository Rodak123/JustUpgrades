using System.Collections.Generic;
using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class AUpgradeSO : UniqueScriptableObject<AUpgradeSO>, IHasDynamicValues
    {
        [SerializeField] private string upgradeName = "Upgrade Name";

        [Space]
        [SerializeField] private UpgradeLevel level;
        [SerializeField] private UpgradeCost[] costs;
        [SerializeField] private UpgradeRequirements requirements;
        [SerializeField] private UpgradeParameters parameters;

        public string UpgradeName => upgradeName;

        public UpgradeLevel Level => level;
        public UpgradeCost[] Costs => costs;
        public UpgradeRequirements Requirements => requirements;
        public UpgradeParameters Parameters => parameters;

        public void Init(UpgradeSystemContext upgradeContext)
        {
            parameters.Init(upgradeContext, this);
        }

        public void Update()
        {
            parameters.Update();
        }

        public IEnumerable<IDynamicValue> GetDynamicValues()
        {
            foreach (IDynamicValue dynamicValue in level.GetDynamicValues())
            {
                yield return dynamicValue;
            }
            foreach (UpgradeCost cost in costs)
            {
                foreach (IDynamicValue dynamicValue in cost.GetDynamicValues())
                {
                    yield return dynamicValue;
                }
            }
            foreach (IDynamicValue dynamicValue in requirements.GetDynamicValues())
            {
                yield return dynamicValue;
            }
            foreach (IDynamicValue dynamicValue in parameters.GetDynamicValues())
            {
                yield return dynamicValue;
            }
        }

        public override string ToString()
        {
            return UpgradePresenter.GetFormattedName(this);
        }

        public string GetChangingValueSuffix()
        {
            return $"upgrade:{UniqueId}";
        }
    }
}