using System;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public abstract class AUpgradeRequirement
    {
        public abstract bool FitsRequirement(AUpgradeSO upgradeSO);
        public abstract string GetInfoText();

        public abstract string GetEditorDescription();
    }
}