namespace Rodak.UpgradeSystem.Utils
{
    public interface IValueScaler
    {
        public BigDouble GetValue(int level, int maxLevel);
        public string Hint();
    }
}