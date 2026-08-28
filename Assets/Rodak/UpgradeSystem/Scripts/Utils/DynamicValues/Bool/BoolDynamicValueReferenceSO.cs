namespace Rodak.UpgradeSystem.Utils
{
    public class BoolDynamicValueReferenceSO : ADynamicValueReferenceSO
    {
        public bool GetBoolValue() => BoolDynamicValue.GetBoolValueFrom(GetValue());

        public override string GetValueString()
        {
            return GetBoolValue().ToString();
        }
    }
}