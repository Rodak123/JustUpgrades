using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class UpgradeSystemExceptionException : Exception
    {
        public UpgradeSystemExceptionException() { }
        public UpgradeSystemExceptionException(string message) : base(message) { }
        public UpgradeSystemExceptionException(string message, Exception inner) : base(message, inner) { }
        protected UpgradeSystemExceptionException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}