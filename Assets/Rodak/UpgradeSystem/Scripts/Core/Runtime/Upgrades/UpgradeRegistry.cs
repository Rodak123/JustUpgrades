using System;
using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeRegistry : MonoBehaviour
    {

        [Serializable]
        private struct UpgradeGroup
        {
            public string groupName;
            public List<AUpgradeSO> upgrades;
        }

        [Header("Grouping only to organize")]
        [SerializeField] private List<UpgradeGroup> upgradeGroups = new();

        private readonly Dictionary<AUpgradeSO, Upgrade> upgrades = new();

        public IEnumerable<Upgrade> Upgrades => upgrades.Values;

        public void Init(UpgradeSystemContext context)
        {
            for (int i = 0; i < upgradeGroups.Count; i++)
            {
                List<AUpgradeSO> groupUpgrades = upgradeGroups[i].upgrades;
                for (int j = 0; j < groupUpgrades.Count; j++)
                {
                    AUpgradeSO upgradeSO = groupUpgrades[j];
                    if (upgradeSO == null)
                    {
                        Debug.LogWarning($"Null upgrade at [{j}] in group: '{upgradeGroups[i].groupName}'");
                        continue;
                    }
                    upgrades.Add(upgradeSO, new(context, upgradeSO));
                }
            }

            // register dynamic values
            foreach (Upgrade upgrade in upgrades.Values)
            {
                List<ADynamicValueReferenceSO> references = new(upgrade.UpgradeSO.GetDynamicValueReferences());
                Debug.Log($"{upgrade} has {references.Count} references");
                foreach (ADynamicValueReferenceSO reference in references)
                {
                    if (reference == null) continue;
                    context.DynamicValuesModifiers.AddDynamicValueReference(reference);
                }
            }

            // init all upgrades
            foreach (Upgrade upgrade in upgrades.Values)
            {
                upgrade.UpgradeSO.Init(context);
            }
        }

        private void Update()
        {
            foreach (Upgrade upgrade in upgrades.Values)
            {
                upgrade.Update();
            }
        }

        public bool TryGetUpgrade(AUpgradeSO targetUpgrade, out Upgrade upgrade)
        {
            if (targetUpgrade == null)
            {
                upgrade = default;
                return false;
            }
            return upgrades.TryGetValue(targetUpgrade, out upgrade);
        }
    }
}