namespace Rodak.UpgradeSystem.Utils
{
    public abstract class AResolvedValue : IResolvedValue
    {
        public string Id { get; private set; }
        public BigDouble BaseAmount { get; private set; }

        public AResolvedValue(string id, BigDouble baseAmount)
        {
            Id = id;
            BaseAmount = baseAmount;
        }

        public override string ToString()
        {
            return $"ResolvedValue[id:{Id}]";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not AResolvedValue other)
                return false;
            return other.Id.Equals(Id);
        }
    }
}