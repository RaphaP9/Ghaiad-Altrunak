using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeCardContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilityUpgradeCardUI abilityUpgradeCardUI;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private Image abilityImage;
    [SerializeField] private TextMeshProUGUI upgradeLevelText;
    [SerializeField] private TextMeshProUGUI upgradeLevelDescription;

    private void OnEnable()
    {
        abilityUpgradeCardUI.OnAbilityUpgradeCardSet += AbilityUpgradeCardUI_OnAbilityUpgradeCardSet;
    }

    private void OnDisable()
    {
        abilityUpgradeCardUI.OnAbilityUpgradeCardSet -= AbilityUpgradeCardUI_OnAbilityUpgradeCardSet;
    }

    private void SetAbilityNameText(string text) => abilityNameText.text = text;
    private void SetAbilitySprite(Sprite sprite) => abilityImage.sprite = sprite;
    private void SetUpgradeLevelText(string text) => upgradeLevelText.text = text;
    private void SetUpgradeLevelDescription(string text) => upgradeLevelDescription.text = text;

    private void CompleteSetUI(AbilityUpgradeCardInfo abilityUpgradeCardInfo, AbilityLevelInfo abilityLevelInfo)
    {
        SetAbilityNameText(abilityUpgradeCardInfo.abilitySO.abilityName);
        SetAbilitySprite(abilityUpgradeCardInfo.abilitySO.sprite);

        SetUpgradeLevelText(MappingUtilities.MapAbilityLevel(abilityUpgradeCardInfo.upgradeLevel));
        SetUpgradeLevelDescription(abilityLevelInfo.cardLevelDescription);
    }

    private void AbilityUpgradeCardUI_OnAbilityUpgradeCardSet(object sender, AbilityUpgradeCardUI.OnAbilityUpgradeCardEventArgs e)
    {
        CompleteSetUI(e.abilityUpgradeCardInfo, e.abilityLevelInfo);
    }
}
