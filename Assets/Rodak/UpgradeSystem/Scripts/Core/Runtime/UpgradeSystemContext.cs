using Rodak.Utils.Singleton;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeSystemContext : SingletonMonoBehaviour<UpgradeSystemContext>
    {
        [SerializeField] private CurrencyWallet currencyWallet;
        [SerializeField] private DynamicValueRegistry dynamicValueRegistry;
        [SerializeField] private UpgradeRegistry upgradeRegistry;

        public CurrencyWallet CurrencyWallet => currencyWallet;
        public DynamicValueRegistry DynamicValueRegistry => dynamicValueRegistry;
        public UpgradeRegistry UpgradeRegistry => upgradeRegistry;

        protected override void Awake()
        {
            base.Awake();

            currencyWallet.Init();
            dynamicValueRegistry.Init();

            upgradeRegistry.Init(this);
        }
    }
}