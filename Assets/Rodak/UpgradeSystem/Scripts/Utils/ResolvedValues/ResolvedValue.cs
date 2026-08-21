using UnityEngine;
using System;

namespace Rodak.UpgradeSystem.Utils
{
    [Serializable]
    public class ResolvedValue
    {
        [SerializeReference] private IResolvedValue resolvedValue = null;

        public ResolvedValue()
        {
            resolvedValue = null;
        }

        public bool HasValue => resolvedValue != null;
    }
}