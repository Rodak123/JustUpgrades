namespace Rodak.UpgradeSystem.Utils
{
    public interface IResolvedValue
    {
        public string Id { get; }
        public BigDouble BaseAmount { get; }
    }
}