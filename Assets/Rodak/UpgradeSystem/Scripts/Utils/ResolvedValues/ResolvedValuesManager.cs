using System.Collections.Generic;

namespace Rodak.UpgradeSystem.Utils
{
    public class ResolvedValuesManager
    {
        private class ResolvedValueHandler
        {
            public readonly IResolvedValue ResolvedValue;

            public ResolvedValueHandler(IResolvedValue resolvedValue)
            {
                ResolvedValue = resolvedValue;
            }

            public BigDouble Query()
            {
                return 0;
            }
        }

        private readonly Dictionary<IResolvedValue, ResolvedValueHandler> handlers = new();

        public void Register(IResolvedValue resolvedValue)
        {
            if (handlers.ContainsKey(resolvedValue))
                throw new UpgradeSystemExceptionException($"{resolvedValue} is already registered.");

            handlers.Add(resolvedValue, new(resolvedValue));
        }

        public BigDouble Query(IResolvedValue resolvedValue)
        {
            if (!handlers.TryGetValue(resolvedValue, out ResolvedValueHandler valueHandler))
                throw new UpgradeSystemExceptionException($"{resolvedValue} is not registered.");

            return valueHandler.Query();
        }
    }
}