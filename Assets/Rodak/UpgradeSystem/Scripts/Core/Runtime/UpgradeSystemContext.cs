using Rodak.UpgradeSystem.Utils;
using Rodak.Utils.Singleton;
using UnityEngine;

namespace Rodak.UpgradeSystem.Core
{
    public class UpgradeSystemContext : SingletonMonoBehaviour<UpgradeSystemContext>
    {
        [SerializeField] private CurrencyWallet currencyWallet;
        [SerializeField] private DynamicValuesModifiers dynamicValuesModifiers;

        public CurrencyWallet CurrencyWallet => currencyWallet;
        public DynamicValuesModifiers DynamicValuesModifiers => dynamicValuesModifiers;

        protected override void Awake()
        {
            base.Awake();

            currencyWallet.Init();
            dynamicValuesModifiers.Init();
        }
    }
}