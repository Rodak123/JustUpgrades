using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    public class UpgradeTreeScene : MonoBehaviour
    {
        public class TestModifier : IDynamicValueModifier
        {
            public void Query(DynamicValueQuery query)
            {
                query.Value += 4;
            }
        }

        [SerializeField] private UTUpgradeSO upgradeSO;

        private Upgrade upgrade;

        private void Start()
        {
            // if (testFloatValue.IsExposed)
            // {
            //     UpgradeSystemContext.Instance.DynamicValuesModifiers.AttachModifier(testFloatValue.ExposedReference, new TestModifier());
            // }

            UpgradeSystemContext.Instance.UpgradeRegistry.TryGetUpgrade(upgradeSO, out upgrade);
        }

        private void Update()
        {
            BigDouble maxLevel = upgradeSO.Level.MaxLevel.GetIntValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);
            Debug.Log($"Max level: {maxLevel}");

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                upgrade.TryUpgrade();
            }
        }
    }
}