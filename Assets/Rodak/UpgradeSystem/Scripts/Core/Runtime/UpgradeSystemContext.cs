using Rodak.Utils.Singleton;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeSystemContext : SingletonMonoBehaviour<UpgradeSystemContext>
    {
        [SerializeField] private DynamicValueRegistry dynamicValueRegistry;
        [SerializeField] private CurrencyWallet currencyWallet;
        [SerializeField] private UpgradeTracker upgradeTracker;

        public DynamicValueRegistry DynamicValueRegistry => dynamicValueRegistry;
        public CurrencyWallet CurrencyWallet => currencyWallet;
        public UpgradeTracker UpgradeTracker => upgradeTracker;

        protected override void Awake()
        {
            base.Awake();

            currencyWallet.Init();
            dynamicValueRegistry.Init();

            upgradeTracker.Init();
        }
    }
}