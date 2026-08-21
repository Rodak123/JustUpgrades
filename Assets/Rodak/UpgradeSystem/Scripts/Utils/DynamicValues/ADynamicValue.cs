using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public abstract class ADynamicValue
    {
        public ADynamicValue()
        { }

        public abstract BigDouble GetBaseAmount();
    }
}