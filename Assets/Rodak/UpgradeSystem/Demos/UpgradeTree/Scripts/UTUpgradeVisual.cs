using System;
using UnityEngine;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    [Serializable]
    public class UTUpgradeVisual
    {
        [SerializeField, TextArea(4, 16)] private string upgradeDescription = "Description";

        public string UpgradeDescription => upgradeDescription;
    }
}