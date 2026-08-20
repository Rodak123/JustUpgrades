using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Rodak.UpgradeSystem.Editor
{
    public class MultiAlertController
    {
        private readonly List<AlertController> alertControllers = new();
        private ScrollView alertsContainer;

        public MultiAlertController(VisualElement root, int heightPixels = 80)
        {
            alertsContainer = new ScrollView();
            alertsContainer.style.height = new StyleLength(heightPixels);

            root.Add(alertsContainer);
        }

        private void ShowAlert(string text, Func<VisualElement, AlertController> alertFactory)
        {
            AlertController newAlert = alertFactory(alertsContainer);
            newAlert.SetText(text);
            newAlert.Show();
            alertControllers.Add(newAlert);
        }

        public void ShowDangerAlert(string text) => ShowAlert(text, AlertEditor.AddDangerAlert);
        public void ShowInfoAlert(string text) => ShowAlert(text, AlertEditor.AddInfoAlert);
        public void ShowSuccessAlert(string text) => ShowAlert(text, AlertEditor.AddSuccessAlert);
        public void ShowWarningAlert(string text) => ShowAlert(text, AlertEditor.AddWarningAlert);

        public void ClearAlerts()
        {
            foreach (AlertController alertController in alertControllers)
            {
                alertController.Delete();
            }
            alertControllers.Clear();
        }
    }
}