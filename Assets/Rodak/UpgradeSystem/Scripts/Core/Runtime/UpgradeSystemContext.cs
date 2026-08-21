using Rodak.Utils.Singleton;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeSystemContext : SingletonMonoBehaviour<UpgradeSystemContext>
    {
        [SerializeField] private CurrencyWallet currencyWallet;
        [SerializeField] private DynamicValuesManager dynamicValuesManager;

        public CurrencyWallet CurrencyWallet => currencyWallet;
        public DynamicValuesManager DynamicValuesManager => dynamicValuesManager;

        protected override void Awake()
        {
            base.Awake();

            currencyWallet.Init();
            dynamicValuesManager.Init();
        }
    }
}