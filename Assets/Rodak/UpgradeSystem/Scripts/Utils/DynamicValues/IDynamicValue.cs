namespace Rodak.UpgradeSystem.Utils
{
    public interface IDynamicValue
    {
        public bool IsExposed { get; }
        public ADynamicValueReferenceSO ExposedReference { get; }
        public IDynamicValueModifier GetBaseModifier();
        public BigDouble ProcessValue(BigDouble value);
    }
}