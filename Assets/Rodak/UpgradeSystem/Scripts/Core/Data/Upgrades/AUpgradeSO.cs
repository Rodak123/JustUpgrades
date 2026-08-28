using System.Collections.Generic;
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
        [SerializeField] private UpgradeRequirement[] requirements;
        [SerializeReference] private AUpgradeParameter[] parameters;

        public string UpgradeName => upgradeName;

        public UpgradeLevel Level => level;
        public UpgradeCost[] Costs => costs;
        public UpgradeRequirement[] Requirements => requirements;
        public AUpgradeParameter[] Parameters => parameters;

        public void Init(UpgradeSystemContext upgradeContext)
        {
            foreach (AUpgradeParameter parameter in parameters)
            {
                parameter.Init(upgradeContext, this);
            }
        }

        public void Update()
        {
            foreach (AUpgradeParameter parameter in parameters)
            {
                parameter.Update();
            }
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
            foreach (UpgradeRequirement requirement in requirements)
            {
                foreach (IDynamicValue dynamicValue in requirement.GetDynamicValues())
                {
                    yield return dynamicValue;
                }
            }
            foreach (AUpgradeParameter parameter in parameters)
            {
                if (parameter is not IHasDynamicValues hasDynamicValues) continue;
                foreach (IDynamicValue dynamicValue in hasDynamicValues.GetDynamicValues())
                {
                    yield return dynamicValue;
                }
            }
        }

        public override string ToString()
        {
            return upgradeName;
        }

        public string GetChangingValueSuffix()
        {
            return $"upgrade:{UniqueId}";
        }
    }
}