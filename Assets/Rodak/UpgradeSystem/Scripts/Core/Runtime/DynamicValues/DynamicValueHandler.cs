using System.Collections.Generic;
using Rodak.UpgradeSystem.Utils;

namespace Rodak.UpgradeSystem.Core
{
    public class DynamicValueHandler
    {
        public readonly IDynamicValue DynamicValue;
        private readonly HashSet<IDynamicValueModifier> valueModifiers = new();

        public DynamicValueHandler(IDynamicValue dynamicValue)
        {
            DynamicValue = dynamicValue;
            AttachModifier(dynamicValue.GetBaseModifier());
        }

        public void AttachModifier(IDynamicValueModifier valueModifier)
        {
            valueModifiers.Add(valueModifier);
        }

        public void RemoveModifier(IDynamicValueModifier valueModifier)
        {
            valueModifiers.Remove(valueModifier);
        }

        public BigDouble QueryValue()
        {
            DynamicValueQuery query = new();
            foreach (IDynamicValueModifier modifier in valueModifiers)
            {
                modifier.Query(query);
            }
            BigDouble value = DynamicValue.ProcessValue(query.Value);
            return value;
        }
    }
}