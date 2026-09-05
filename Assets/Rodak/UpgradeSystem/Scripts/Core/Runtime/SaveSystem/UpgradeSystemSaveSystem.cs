using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeSystemSaveSystem : MonoBehaviour, IHasSaveState
    {
        private const string PlayerPrefsSaveKey = "RODAK_UPGRADE_SYSTEM_SAVE";

        private class SaveState
        {
            public JObject UpgradeTrackerState;
            public JObject CurrencyWalletState;
        }

        public static string SanitizeName(string name)
        {
            return name
                .Trim()
                .ToLower()
                .Replace(" ", "_");
        }

        public JObject CreateSaveState()
        {
            UpgradeSystemContext upgradeContext = UpgradeSystemContext.Instance;

            SaveState saveState = new()
            {
                UpgradeTrackerState = upgradeContext.UpgradeTracker.CreateSaveState(),
                CurrencyWalletState = upgradeContext.CurrencyWallet.CreateSaveState(),
            };

            return JObject.FromObject(saveState);
        }

        public void LoadSaveState(JObject saveStateObject)
        {
            if (saveStateObject == null) return;

            SaveState saveState = saveStateObject.ToObject<SaveState>();

            if (saveState == null)
            {
                Debug.LogError($"Trying to load invalid save state: '{saveStateObject}'");
                return;
            }

            UpgradeSystemContext upgradeContext = UpgradeSystemContext.Instance;

            upgradeContext.UpgradeTracker.LoadSaveState(saveState.UpgradeTrackerState);
            upgradeContext.CurrencyWallet.LoadSaveState(saveState.CurrencyWalletState);
        }

        public string CreateSaveJSONString()
        {
            JObject saveStateObject = CreateSaveState();
            return saveStateObject.ToString();
        }

        public void LoadSaveJSONString(string saveJSONString)
        {
            JObject saveStateObject = JObject.Parse(saveJSONString);
            LoadSaveState(saveStateObject);
        }

        public void Save()
        {
            string json = CreateSaveJSONString();
            Debug.Log($"Saving:\n{json}");

            PlayerPrefs.SetString(PlayerPrefsSaveKey, json);
        }

        public void Load()
        {
            string json = PlayerPrefs.GetString(PlayerPrefsSaveKey, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Nothing saved in player prefs.");
                return;
            }

            Debug.Log($"Loading:\n{json}");

            LoadSaveJSONString(json);
        }
    }
}