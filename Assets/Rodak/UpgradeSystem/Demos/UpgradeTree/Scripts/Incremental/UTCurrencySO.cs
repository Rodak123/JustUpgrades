using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [CreateAssetMenu(menuName = "UpgradeSystem/Demo UT/Currency", order = 110)]
    public class UTCurrencySO : ACurrencySO
    {
        [SerializeField] private BigDouble baseGain = 1;

        public BigDouble BaseGain => baseGain;
    }
}