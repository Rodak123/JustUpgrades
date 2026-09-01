using System;

namespace Rodak.UpgradeSystem.Core
{
    [Serializable]
    public abstract class AUpgradeParameter
    {
        public abstract void Init(AUpgradeSO upgradeSO);
        public abstract void Update();
        public abstract string GetInfoText();

        public abstract string GetEditorDescription();
    }
}