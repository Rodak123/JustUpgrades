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

        [SerializeField] private FloatDynamicValue testFloatValue = new();
        [SerializeField] private FloatDynamicValue fixedValue = new();

        [Space]
        [SerializeField] private UpgradeSO upgradeSO;

        [Space]
        [SerializeField] private ScaledValue coolValue;
        [SerializeField] private ScaledValue coolerValue;

        private void Start()
        {
            if (testFloatValue.IsExposed)
            {
                UpgradeSystemContext.Instance.DynamicValuesModifiers.AttachModifier(testFloatValue.ExposedReference, new TestModifier());
            }
        }

        private void Update()
        {
            BigDouble myValue = testFloatValue.GetFloatValue(UpgradeSystemContext.Instance.DynamicValuesModifiers);
            Debug.Log($"My Test Float Value: {myValue}");
        }
    }
}