using Rodak.UpgradeSystem.Core;

namespace Rodak.UpgradeSystem.Utils
{
    public abstract class ADynamicValueReferenceSO : UniqueScriptableObject<ADynamicValueReferenceSO>
    {
        public BigDouble GetValue()
        {
            if (!UpgradeSystemContext.HasInstance) return 0;
            return UpgradeSystemContext.Instance.DynamicValueRegistry.QueryValue(this);
        }

        public virtual string GetValueString()
        {
            return $"{GetValue()}";
        }

        public sealed override string ToString()
        {
            return name;
        }
    }
}