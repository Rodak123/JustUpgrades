using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    public class UTCurrencyListUI : MonoBehaviour
    {
        [SerializeField] private UTCurrencyGenerator[] currencyGenerators;

        [Header("UI")]
        [SerializeField] private UTCurrencyUI currencyUIPrefab;

        private void Awake()
        {
            foreach (UTCurrencyGenerator currencyGenerator in currencyGenerators)
            {
                UTCurrencyUI currencyUI = Instantiate(currencyUIPrefab, transform);
                currencyUI.TargetGenerator = currencyGenerator;
            }
        }
    }
}