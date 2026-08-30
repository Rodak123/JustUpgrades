using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public class UpgradeParameters : IHasDynamicValues
    {
        [SerializeReference] private AUpgradeParameter[] parameters;

        public AUpgradeParameter[] Parameters => parameters;

        public void Init(UpgradeSystemContext upgradeContext, AUpgradeSO upgradeSO)
        {
            foreach (AUpgradeParameter parameter in parameters)
            {
                parameter.Init(upgradeContext, upgradeSO);
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
            foreach (AUpgradeParameter parameter in parameters)
            {
                if (parameter is not IHasDynamicValues hasDynamicValues) continue;
                foreach (IDynamicValue dynamicValue in hasDynamicValues.GetDynamicValues())
                {
                    yield return dynamicValue;
                }
            }
        }
    }
}