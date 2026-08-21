namespace Rodak.UpgradeSystem.Utils
{
    public abstract class ADynamicValueReferenceSO : UniqueScriptableObject<ADynamicValueReferenceSO>
    {
        public override string ToString()
        {
            return name;
        }
    }
}