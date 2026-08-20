
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    public static class AlertEditor
    {
        public static AlertController AddDangerAlert(VisualElement root)
        {
            Color32 backgroundColor = new(0x6E, 0x1B, 0x23, 0xFF);
            Color32 textColor = new(0xFF, 0xD3, 0xD0, 0xFF);
            Color32 borderColor = new(0xFF, 0x00, 0x00, 0xFF);
            return new(root, backgroundColor, textColor, borderColor);
        }

        public static AlertController AddSuccessAlert(VisualElement root)
        {
            Color32 backgroundColor = new(0x1B, 0x6E, 0x23, 0xFF);
            Color32 textColor = new(0xDE, 0xFF, 0xD0, 0xFF);
            Color32 borderColor = new(0x00, 0xFF, 0x00, 0xFF);
            return new(root, backgroundColor, textColor, borderColor);
        }

        public static AlertController AddInfoAlert(VisualElement root)
        {
            Color32 backgroundColor = new(0x1B, 0x3B, 0x6E, 0xFF);
            Color32 textColor = new(0xD0, 0xE4, 0xFF, 0xFF);
            Color32 borderColor = new(0x00, 0x80, 0xFF, 0xFF);
            return new(root, backgroundColor, textColor, borderColor);
        }

        public static AlertController AddWarningAlert(VisualElement root)
        {
            Color32 backgroundColor = new(0x6E, 0x4B, 0x1B, 0xFF);
            Color32 textColor = new(0xFF, 0xFD, 0xD0, 0xFF);
            Color32 borderColor = new(0xFF, 0xA5, 0x00, 0xFF);
            return new(root, backgroundColor, textColor, borderColor);
        }
    }
}