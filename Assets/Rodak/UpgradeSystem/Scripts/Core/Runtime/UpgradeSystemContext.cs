using Rodak.UpgradeSystem.Utils;
using Rodak.Utils.Singleton;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeSystemContext : SingletonMonoBehaviour<UpgradeSystemContext>
    {
        [SerializeField] private CurrencyWallet currencyWallet;
        [SerializeField] private DynamicValuesModifiers dynamicValuesModifiers;
        [SerializeField] private UpgradeRegistry upgradeRegistry;

        public CurrencyWallet CurrencyWallet => currencyWallet;
        public DynamicValuesModifiers DynamicValuesModifiers => dynamicValuesModifiers;
        public UpgradeRegistry UpgradeRegistry => upgradeRegistry;

        protected override void Awake()
        {
            base.Awake();

            currencyWallet.Init();
            dynamicValuesModifiers.Init();

            upgradeRegistry.Init(this);
        }
    }
}