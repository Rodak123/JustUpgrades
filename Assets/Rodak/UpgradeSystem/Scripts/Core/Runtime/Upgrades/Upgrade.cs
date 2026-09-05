using System;
using Newtonsoft.Json.Linq;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class Upgrade : IHasSaveState
    {
        private class SaveState
        {
            public int CurrentLevel;
        }

        private int currentLevel = 0;

        private readonly UpgradeSystemContext upgradeContext;
        public readonly AUpgradeSO UpgradeSO;

        public int CurrentLevel => currentLevel;
        public bool IsInfinite => UpgradeSO.Level.IsInfinite.GetBoolValue();
        public int MaxLevel => IsInfinite ? currentLevel + 1 : UpgradeSO.Level.MaxLevel.GetIntValue();
        public int NextLevel => Mathf.Min(currentLevel + 1, MaxLevel);

        public event Action<Upgrade> OnUpgraded;
        public event Action<Upgrade> OnReset;

        public Upgrade(AUpgradeSO upgradeSO)
        {
            upgradeContext = UpgradeSystemContext.Instance;
            UpgradeSO = upgradeSO;
        }

        public void Update()
        {
            UpgradeSO.Update();
        }

        public bool TryUpgrade()
        {
            if (!CanBeUpgraded())
                return false;

            foreach (UpgradeCost cost in UpgradeSO.Costs.Costs)
            {
                ACurrencySO currency = cost.TargetCurrency;

                if (currency == null) continue;
                BigDouble costValue = cost.GetCostValue(currentLevel, MaxLevel);
                bool isSpent = cost.IsSpent.GetBoolValue();

                if (isSpent && !upgradeContext.CurrencyWallet.TrySpend(currency, costValue))
                    throw new Exception($"Failed to purchase upgrade '{UpgradeSO}'");
            }

            currentLevel++;
            OnUpgraded?.Invoke(this);
            return true;
        }

        public void ResetTo(int resetLevel)
        {
            if (resetLevel < 0) resetLevel = 0;
            currentLevel = resetLevel;
            OnReset?.Invoke(this);
        }

        public bool CanBeUpgraded()
        {
            if (!CanImprove())
                return false;

            if (!CanFitRequirements())
                return false;

            if (!CanAfford())
                return false;

            return true;
        }

        public bool CanImprove()
        {
            return currentLevel < MaxLevel;
        }

        public bool CanFitRequirements()
        {
            return UpgradeSO.Requirements.FitsRequirements(UpgradeSO);
        }

        public bool CanAfford()
        {
            return UpgradeSO.Costs.CanAffordAt(currentLevel, MaxLevel);
        }

        public JObject CreateSaveState()
        {
            SaveState saveState = new()
            {
                CurrentLevel = CurrentLevel,
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

            currentLevel = saveState.CurrentLevel;
        }
    }
}