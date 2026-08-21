using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Utils;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.Demo1
{
    public class Demo1Scene : MonoBehaviour
    {
        public class TestModifier : IDynamicValueModifier
        {
            public void Query(DynamicValueQuery query)
            {
                query.Value += 4;
            }
        }

        [SerializeReference, MarkDynamicValue(ValueName = "TestFloatValue")] private FloatDynamicValue testFloatValue = new();
        [SerializeReference] private FloatDynamicValue fixedValue = new();

        [Space]
        [SerializeField] private UpgradeSO upgradeSO;

        [Space]
        [SerializeField] private ScaledValue coolValue;

        private void Start()
        {
            if (testFloatValue.IsExposed)
            {
                UpgradeSystemContext.Instance.DynamicValuesManager.AttachModifier(testFloatValue.ExposedReference, new TestModifier());
            }

            if (upgradeSO.TestFloatValue.IsExposed)
            {
                UpgradeSystemContext.Instance.DynamicValuesManager.AttachModifier(upgradeSO.TestFloatValue.ExposedReference, new TestModifier());
            }
        }

        private void Update()
        {
            BigDouble myValue = UpgradeSystemContext.Instance.DynamicValuesManager.QueryFloat(testFloatValue);
            Debug.Log($"My Test Float Value: {myValue}");

            BigDouble upgradeValue = UpgradeSystemContext.Instance.DynamicValuesManager.QueryFloat(upgradeSO.TestFloatValue);
            Debug.Log($"Upgrade Test Float Value: {upgradeValue}");
        }
    }
}