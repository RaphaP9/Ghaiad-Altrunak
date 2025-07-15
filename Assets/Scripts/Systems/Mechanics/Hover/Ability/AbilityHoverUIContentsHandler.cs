using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHoverUIContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilityHoverUIHandler abilityHoverUIHandler;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private Image abilityImage;
    [SerializeField] private TextMeshProUGUI abilityDescriptionText;
    [SerializeField] private TextMeshProUGUI abilityLevelText;
    [SerializeField] private TextMeshProUGUI abilityTypeText;
    [Space]
    [SerializeField] private RectTransform abilityCooldownGroup;
    [SerializeField] private TextMeshProUGUI abilityCooldownText;

    private void OnEnable()
    {
        abilityHoverUIHandler.OnAbilitySet += AbilityHoverUIHandler_OnAbilitySet;
    }

    private void OnDisable()
    {
        abilityHoverUIHandler.OnAbilitySet -= AbilityHoverUIHandler_OnAbilitySet;
    }

    private void CompleteSetUI(Ability ability)
    {
        SetAbilityNameText(ability);    
        SetAbilityImage(ability);
        SetAbilityDescriptionText(ability);
        SetAbilityLevelText(ability);
        SetAbilityTypeText(ability);

        SetAbilityCooldownText(ability);
    }

    private void SetAbilityNameText(Ability ability) => abilityNameText.text = ability.AbilitySO.abilityName;
    private void SetAbilityImage(Ability ability) => abilityImage.sprite = ability.AbilitySO.sprite;
    private void SetAbilityDescriptionText(Ability ability)
    {
        AbilityLevel abilityLevel = ability.AbilityLevel;
        AbilityLevelInfo abilityLevelInfo = ability.AbilitySO.abilityInfoSO.GetAbilityLevelInfoByLevel(abilityLevel);
        abilityDescriptionText.text = abilityLevelInfo.levelDescription;
    }

    private void SetAbilityLevelText(Ability ability) => abilityLevelText.text = MappingUtilities.MapAbilityLevel(ability.AbilityLevel);
    private void SetAbilityTypeText(Ability ability) => abilityTypeText.text = MappingUtilities.MapAbilityType(ability.AbilitySO.GetAbilityType());

    private void SetAbilityCooldownText(Ability ability)
    {
        switch (ability.AbilitySO.GetAbilityType())
        {
            case AbilityType.Passive:
                HandlePassiveAbilityCooldown(ability as PassiveAbility);
                break;
            case AbilityType.Active:
                HandleActiveAbilityCooldown(ability as ActiveAbility);
                break;
            case AbilityType.ActivePassive:
                HandleActivePassiveAbilityCooldown(ability as ActivePassiveAbility);
                break;

        }
    }

    private void HandlePassiveAbilityCooldown(PassiveAbility passiveAbility)
    {
        DisableAbilityCooldownGroup();
    }

    private void HandleActiveAbilityCooldown(ActiveAbility activeAbility)
    {
        EnableAbilityCooldownGroup();
        float floatCooldown = GeneralUtilities.RoundToNDecimalPlaces(activeAbility.ProcessedAbilityCooldown, 1);

        abilityCooldownText.text = UIUtilities.ProcessFloatToString(floatCooldown);
    }

    private void HandleActivePassiveAbilityCooldown(ActivePassiveAbility activePassiveAbility)
    {
        EnableAbilityCooldownGroup();
        float floatCooldown = GeneralUtilities.RoundToNDecimalPlaces(activePassiveAbility.ProcessedAbilityCooldown, 1);

        abilityCooldownText.text = UIUtilities.ProcessFloatToString(floatCooldown);
    }

    private void EnableAbilityCooldownGroup() => abilityCooldownGroup.gameObject.SetActive(true);
    private void DisableAbilityCooldownGroup() => abilityCooldownGroup.gameObject.SetActive(false);

    private void AbilityHoverUIHandler_OnAbilitySet(object sender, AbilityHoverUIHandler.OnAbilityEventArgs e)
    {
        CompleteSetUI(e.ability);
    }
}
