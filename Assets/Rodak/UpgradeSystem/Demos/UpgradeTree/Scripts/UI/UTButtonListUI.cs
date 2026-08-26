using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    public class UTButtonListUI : MonoBehaviour
    {
        [SerializeField] private UTUpgradeSO[] upgrades;

        [Header("Button")]
        [SerializeField] private float spacing = 2;
        [SerializeField] private UTButtonUI buttonUIPrefab;

        private void Awake()
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                UTButtonUI buttonUI = Instantiate(buttonUIPrefab, transform);
                buttonUI.TargetUpgrade = upgrades[i];

                buttonUI.transform.localPosition = new(0, spacing * i, 0);
            }
        }
    }
}