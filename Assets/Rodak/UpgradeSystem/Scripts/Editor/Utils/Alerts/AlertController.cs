using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    public class AlertController
    {
        private const float FontSize = 16;
        private const float BorderWidth = 2;
        private const float BorderRadius = 8;
        private const float Padding = 4;
        private const float Margin = 8;

        private VisualElement alertContainer;
        private Label alertLabel;

        private bool isHidden = true;

        public bool IsHidden => isHidden;

        public AlertController(VisualElement root, Color backgroundColor, Color textColor, Color borderColor)
        {
            alertContainer = new VisualElement();

            alertContainer.style.backgroundColor = new StyleColor(backgroundColor);
            alertContainer.style.borderLeftColor = new StyleColor(borderColor);
            alertContainer.style.borderRightColor = new StyleColor(borderColor);
            alertContainer.style.borderTopColor = new StyleColor(borderColor);
            alertContainer.style.borderBottomColor = new StyleColor(borderColor);

            alertContainer.style.borderLeftWidth = BorderWidth;
            alertContainer.style.borderRightWidth = BorderWidth;
            alertContainer.style.borderTopWidth = BorderWidth;
            alertContainer.style.borderBottomWidth = BorderWidth;

            alertContainer.style.borderTopLeftRadius = BorderRadius;
            alertContainer.style.borderTopRightRadius = BorderRadius;
            alertContainer.style.borderBottomLeftRadius = BorderRadius;
            alertContainer.style.borderBottomRightRadius = BorderRadius;

            alertContainer.style.paddingLeft = Padding;
            alertContainer.style.paddingRight = Padding;
            alertContainer.style.paddingTop = Padding;
            alertContainer.style.paddingBottom = Padding;

            alertContainer.style.marginLeft = Margin;
            alertContainer.style.marginRight = Margin;
            alertContainer.style.marginTop = Margin;
            alertContainer.style.marginBottom = Margin;

            alertLabel = new Label("Default Alert");
            alertContainer.Add(alertLabel);

            alertLabel.style.color = new StyleColor(textColor);
            alertLabel.style.fontSize = new StyleLength(FontSize);
            alertLabel.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Normal);
            alertLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            alertLabel.style.whiteSpace = WhiteSpace.Normal;

            root.Add(alertContainer);

            Hide();
        }

        public void SetText(string text)
        {
            alertLabel.text = text;
        }

        public void Hide()
        {
            isHidden = true;
            alertContainer.style.opacity = 0;
            alertContainer.style.scale = new StyleScale(new Vector2(0.9f, 0.9f));
            alertContainer.RegisterCallback<TransitionEndEvent>(OnHidden);
        }

        private void OnHidden(TransitionEndEvent evt)
        {
            if (isHidden) alertContainer.style.display = DisplayStyle.None;
            alertContainer.UnregisterCallback<TransitionEndEvent>(OnHidden);
        }

        public void Show()
        {
            isHidden = false;
            alertContainer.style.display = DisplayStyle.Flex;

            alertContainer.schedule.Execute(() =>
            {
                alertContainer.style.opacity = 1;
                alertContainer.style.scale = new StyleScale(new Vector2(1f, 1f));
            }).ExecuteLater(10);
        }

        public void Delete()
        {
            alertContainer.RemoveFromHierarchy();
        }
    }
}