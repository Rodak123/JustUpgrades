using System;

namespace Rodak.UpgradeSystem.Core
{
    /// <summary>
    /// Allows the upgrade requirement to be shown in the creation menu.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CreateUpgradeRequirementMenuAttribute : Attribute
    {
        public bool ShowRequirement { get; set; } = true;
        public string MenuName { get; set; }
        public int Order { get; set; } = 0;
    }
}