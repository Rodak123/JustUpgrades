namespace Rodak.UpgradeSystem.Utils
{
    public class BoolResolvedValue : AResolvedValue
    {
        public BoolResolvedValue(string id, bool baseAmount) : base(id, baseAmount ? 0 : 1)
        { }
    }
}