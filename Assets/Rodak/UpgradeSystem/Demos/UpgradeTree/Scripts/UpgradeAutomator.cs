using System.Collections.Generic;
using Rodak.UpgradeSystem.Core;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    public class UpgradeAutomator : MonoBehaviour
    {
        private readonly List<Upgrade> automatableUpgrades = new();

        private void Start()
        {
            foreach (Upgrade upgrade in UpgradeSystemContext.Instance.UpgradeTracker.Upgrades)
            {
                if (upgrade.UpgradeSO is not UTUpgradeSO upgradeSO)
                {
                    continue;
                }

                automatableUpgrades.Add(upgrade);
            }
        }

        private void Update()
        {
            foreach (Upgrade upgrade in automatableUpgrades)
            {
                bool isAutomated = ((UTUpgradeSO)upgrade.UpgradeSO).IsAutomated.GetBoolValue();
                if (!isAutomated) continue;

                upgrade.TryUpgrade();
            }
        }
    }
}