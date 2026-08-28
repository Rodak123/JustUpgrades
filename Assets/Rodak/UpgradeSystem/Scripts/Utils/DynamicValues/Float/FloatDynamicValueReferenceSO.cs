namespace Rodak.UpgradeSystem.Utils
{
    public class FloatDynamicValueReferenceSO : ADynamicValueReferenceSO
    {
        public BigDouble GetFloatValue() => FloatDynamicValue.GetFloatValueFrom(GetValue());

        public override string GetValueString()
        {
            return GetFloatValue().ToString();
        }
    }
}