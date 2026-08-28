namespace Rodak.UpgradeSystem.Utils
{
    public class IntDynamicValueReferenceSO : ADynamicValueReferenceSO
    {
        public int GetIntValue() => IntDynamicValue.GetIntValueFrom(GetValue());

        public override string GetValueString()
        {
            return GetIntValue().ToString();
        }
    }
}