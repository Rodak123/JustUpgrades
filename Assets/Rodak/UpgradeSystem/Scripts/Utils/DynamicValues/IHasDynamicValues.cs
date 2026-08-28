using System.Collections.Generic;

namespace Rodak.UpgradeSystem.Utils
{
    public interface IHasDynamicValues
    {
        public IEnumerable<IDynamicValue> GetDynamicValues();
    }
}