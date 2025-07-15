using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilitySlotHandler abilitySlotHandler;
    [Space]
    [SerializeField] private AbilityCooldownUIHandler abilityCooldownUIHandler;
    [SerializeField] private AbilityLevelUIHandler abilityLevelUIHandler;
    [SerializeField] private AbilityKeyBindUIHandler abilityKeyBindUIHandler;
    [SerializeField] private AbilityHoverHandler abilityHoverUIHandler;

    [Header("UI Components")]
    [SerializeField] private Image abilityImage; 

    private void OnEnable()
    {
        if (abilitySlotHandler == null) return;

        abilitySlotHandler.OnAbilityVariantInitialized += AbilitySlotHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected += AbilitySlotHandler_OnAbilityVariantSelected;
    }

    private void OnDisable()
    {
        if (abilitySlotHandler == null) return;

        abilitySlotHandler.OnAbilityVariantInitialized -= AbilitySlotHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected -= AbilitySlotHandler_OnAbilityVariantSelected;
    }

    private void Awake()
    {
        InjectKeyBindAbilitySlot();
    }

    private void SetAbilityImage(Sprite sprite) => abilityImage.sprite = sprite;

    private void InjectKeyBindAbilitySlot()
    {
        if (abilitySlotHandler == null) return;
        abilityKeyBindUIHandler.SetAbilitySlot(abilitySlotHandler.AbilitySlot);
    }

    private void AbilitySlotHandler_OnAbilityVariantInitialized(object sender, AbilitySlotHandler.OnAbilityVariantInitializationEventArgs e)
    {
        SetAbilityImage(e.abilityVariant.AbilitySO.sprite);
        abilityCooldownUIHandler.AssignAbility(e.abilityVariant);
        abilityLevelUIHandler.AssignAbility(e.abilityVariant);
        abilityKeyBindUIHandler.AssignAbility(e.abilityVariant);
        abilityHoverUIHandler.AssignAbility(e.abilityVariant);
    }

    private void AbilitySlotHandler_OnAbilityVariantSelected(object sender, AbilitySlotHandler.OnAbilityVariantSelectionEventArgs e)
    {
        SetAbilityImage(e.newAbilityVariant.AbilitySO.sprite);
        abilityCooldownUIHandler.AssignAbility(e.newAbilityVariant);
        abilityLevelUIHandler.AssignAbility(e.newAbilityVariant);
        abilityKeyBindUIHandler.AssignAbility(e.newAbilityVariant);
        abilityHoverUIHandler.AssignAbility(e.newAbilityVariant);
    }
}
