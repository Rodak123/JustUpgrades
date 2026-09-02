using System;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable, CreateUpgradeParameterMenu(Order = 110, MenuName = "Ascension")]
    public class AscensionUpgradeParameter : AUpgradeParameter
    {
        [SerializeField] private AAscension targetAscension;

        private Upgrade upgrade;

        private void Upgrade_OnUpgraded(Upgrade upgrade)
        {
            Ascend();
        }

        protected virtual void Ascend()
        {
            UpgradeSystemContext upgradeContext = UpgradeSystemContext.Instance;

            foreach (UpgradeReset upgradeReset in targetAscension.UpgradeResets)
            {
                int resetLevel = upgradeReset.ResetLevel.GetIntValue();

                foreach (AUpgradeSO upgradeSO in upgradeReset.TargetUpgradeContainer)
                {
                    if (!upgradeContext.UpgradeTracker.TryGetUpgrade(upgradeSO, out Upgrade upgrade))
                    {
                        Debug.LogWarning($"Upgrade '{upgradeSO}' was not reset.");
                        continue;
                    }

                    upgrade.ResetTo(resetLevel);
                }
            }

            foreach (CurrencyReset currencyReset in targetAscension.CurrencyResets)
            {
                BigDouble resetAmount = currencyReset.ResetAmount.GetFloatValue();

                foreach (ACurrencySO currencySO in currencyReset.TargetCurrencyContainer)
                {
                    upgradeContext.CurrencyWallet.SetCurrency(currencySO, resetAmount);
                }
            }
        }

        public override void Init(AUpgradeSO upgradeSO)
        {
            if (targetAscension == null)
            {
                Debug.LogError($"Can't initialize parameter '{nameof(AscensionUpgradeParameter)}': {nameof(targetAscension)} is null. In: {upgradeSO}");
                return;
            }

            UpgradeSystemContext.Instance.UpgradeTracker.TryGetUpgrade(upgradeSO, out upgrade);

            upgrade.OnUpgraded += Upgrade_OnUpgraded;
        }

        public override void Update() { }

        public override string GetInfoText()
        {
            return string.Empty;
        }

        public override string GetEditorDescription()
        {
            if (targetAscension == null) return $"Missing '{nameof(targetAscension)}'!";
            return $"Completes '{targetAscension}'";
        }
    }
}