using System;

namespace Rodak.UpgradeSystem.Utils
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class MarkDynamicValueAttribute : Attribute
    {
        public string ValueName { get; set; }
    }

}