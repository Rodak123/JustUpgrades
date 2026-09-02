using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeTracker : MonoBehaviour, IHasSaveState
    {
        [Serializable]
        private class SaveState
        {
            public Dictionary<string, JObject> UpgradeSaveStates;
        }

        [Serializable]
        private struct UpgradeGroup
        {
            public string groupName;
            public List<AUpgradeContainerSO> upgrades;
        }

        [Header("Grouping only to organize")]
        [SerializeField] private List<UpgradeGroup> upgradeGroups = new();

        private readonly Dictionary<AUpgradeSO, Upgrade> upgrades = new();

        public IEnumerable<Upgrade> Upgrades => upgrades.Values;

        private bool isInitialized;

        public void Init()
        {
            UpgradeSystemContext upgradeContext = UpgradeSystemContext.Instance;

            for (int i = 0; i < upgradeGroups.Count; i++)
            {
                List<AUpgradeContainerSO> upgradeContainers = upgradeGroups[i].upgrades;
                for (int j = 0; j < upgradeContainers.Count; j++)
                {
                    foreach (AUpgradeSO upgradeSO in upgradeContainers[j])
                    {
                        if (upgradeSO == null)
                        {
                            Debug.LogWarning($"Null upgrade at [{j}] in group: '{upgradeGroups[i].groupName}'");
                            continue;
                        }
                        upgrades.Add(upgradeSO, new(upgradeSO));
                    }
                }
            }

            // register dynamic values
            foreach (Upgrade upgrade in upgrades.Values)
            {
                upgradeContext.DynamicValueRegistry.RegisterDynamicValuesFrom(upgrade.UpgradeSO);
            }

            // init all upgrades
            foreach (Upgrade upgrade in upgrades.Values)
            {
                upgrade.UpgradeSO.Init();
            }

            isInitialized = true;
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

        public JObject CreateSaveState()
        {
            SaveState saveState = new()
            {
                UpgradeSaveStates = new(),
            };

            foreach (AUpgradeSO upgradeSO in upgrades.Keys)
            {
                JObject upgradeSaveState = upgrades[upgradeSO].CreateSaveState();
                saveState.UpgradeSaveStates.Add(upgradeSO.SaveId, upgradeSaveState);
            }

            return JObject.FromObject(saveState);
        }

        public void LoadSaveState(JObject saveStateObject)
        {
            if (!isInitialized)
            {
                Debug.LogError($"Can't load state before initialization");
                return;
            }

            if (saveStateObject == null) return;

            SaveState saveState = saveStateObject.ToObject<SaveState>();

            if (saveState == null)
            {
                Debug.LogError($"Trying to load invalid save state: '{saveStateObject}'");
                return;
            }

            foreach (string upgradeKey in saveState.UpgradeSaveStates.Keys)
            {
                JObject upgradeSaveState = saveState.UpgradeSaveStates[upgradeKey];

                AUpgradeSO targetUpgradeSO = null;
                foreach (AUpgradeSO upgradeSO in upgrades.Keys)
                {
                    if (upgradeSO.SaveId == upgradeKey)
                    {
                        targetUpgradeSO = upgradeSO;
                        break;
                    }
                }

                if (targetUpgradeSO == null)
                {
                    Debug.LogWarning($"Failed to load upgrade: '{upgradeKey}', upgrade not registered, ignoring");
                    continue;
                }

                upgrades[targetUpgradeSO].LoadSaveState(upgradeSaveState);
            }
        }
    }
}