using System.Collections.Generic;
using Rodak.UpgradeSystem.UI;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public abstract class AUpgradeSO : AUpgradeContainerSO, IHasDynamicValues
    {
        [SerializeField] private string saveId;
        [SerializeField] private string upgradeName = "Upgrade Name";

        [Space]
        [SerializeField] private UpgradeLevel level;
        [SerializeField] private UpgradeCosts costs;
        [SerializeField] private UpgradeRequirements requirements;
        [SerializeField] private UpgradeParameters parameters;

        public string SaveId => saveId;
        public string UpgradeName => upgradeName;

        public UpgradeLevel Level => level;
        public UpgradeCosts Costs => costs;
        public UpgradeRequirements Requirements => requirements;
        public UpgradeParameters Parameters => parameters;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(saveId))
            {
                Debug.LogWarning($"Missing saveId for '{name}'");
            }
        }
#endif

        public void Init()
        {
            parameters.Init(this);
        }

        public void Update()
        {
            parameters.Update();
        }

        public virtual IEnumerable<IDynamicValue> GetDynamicValues()
        {
            foreach (IDynamicValue dynamicValue in level.GetDynamicValues())
            {
                yield return dynamicValue;
            }
            foreach (IDynamicValue dynamicValue in costs.GetDynamicValues())
            {
                yield return dynamicValue;
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

        public override IEnumerable<AUpgradeSO> GetUpgradeSOs()
        {
            yield return this;
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