namespace Rodak.UpgradeSystem.Utils
{
    public class IntResolvedValue : AResolvedValue
    {
        public IntResolvedValue(string id, BigDouble baseAmount) : base(id, baseAmount.Floor())
        { }
    }
}