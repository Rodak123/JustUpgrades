using Rodak.UpgradeSystem.Core;
using Rodak.UpgradeSystem.Demos.Shared;
using Rodak.UpgradeSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rodak.UpgradeSystem.Demos.UpgradeTree
{
    public class UTButtonUI : MonoBehaviour
    {
        public UTUpgradeSO TargetUpgrade;

        [Header("Visuals")]
        [SerializeField] private Color lockedColor = Color.gray;
        [SerializeField] private Color completedColor = Color.green;
        [SerializeField] private Color upgradedColor = Color.yellow;
        [SerializeField] private Color notUpgradedColor = Color.red;

        [Header("Components")]
        [SerializeField] private CustomButton button;
        [SerializeField] private Image border;
        [SerializeField] private GameObject lockedContainer;
        [SerializeField] private GameObject purchasableOutline;
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text parameterText;
        [SerializeField] private GameObject costContainer;
        [SerializeField] private GameObject maxStatusContainer;

        private UpgradeSystemContext upgradeContext;
        private Upgrade upgrade;

        private void Start()
        {
            upgradeContext = UpgradeSystemContext.Instance;

            if (!upgradeContext.UpgradeTracker.TryGetUpgrade(TargetUpgrade, out upgrade))
            {
                Debug.LogError($"{nameof(Upgrade)} {TargetUpgrade} not registered.");
                gameObject.SetActive(false);
                return;
            }

            button.OnClick += CustomButton_OnClick;
            button.OnHold += CustomButton_OnHold;
            UpdateUI();
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            lockedContainer.SetActive(!upgrade.CanFitRequirements());

            button.Intractable = upgrade.CanBeUpgraded();
            purchasableOutline.SetActive(upgrade.CanBeUpgraded());

            numberText.text = $"#{TargetUpgrade.UpgradeNumber}";
            nameText.text = $"{UpgradePresenter.GetFormattedName(upgrade.UpgradeSO)}";
            descriptionText.text = TargetUpgrade.UpgradeDescription;
            levelText.text = $"Level\n{UpgradePresenter.GetFormattedLevel(upgrade)}";
            costText.text = $"{UpgradePresenter.GetFormattedCosts(upgrade)}";

            bool canImprove = upgrade.CanImprove();
            costContainer.SetActive(canImprove);
            maxStatusContainer.SetActive(!canImprove);

            border.color = GetStatusColor();

            parameterText.text = GetParameterInfo();
        }

        private string GetParameterInfo()
        {
            string info = "";

            if (upgrade.CurrentLevel == 0)
                return info;

            foreach (AUpgradeParameter parameter in TargetUpgrade.Parameters.Parameters)
            {
                info += $"{parameter.GetInfoText()} ";
            }

            return info.Trim();
        }

        private Color GetStatusColor()
        {
            if (!upgrade.CanImprove())
                return completedColor;
            if (!upgrade.CanFitRequirements())
                return lockedColor;
            return upgrade.CurrentLevel > 0 ? upgradedColor : notUpgradedColor;
        }

        private void CustomButton_OnClick(CustomButton customButton, PointerEventData.InputButton button)
        {
            upgrade.TryUpgrade();
        }

        private void CustomButton_OnHold(CustomButton customButton, PointerEventData.InputButton button)
        {
            upgrade.TryUpgrade();
        }
    }
}