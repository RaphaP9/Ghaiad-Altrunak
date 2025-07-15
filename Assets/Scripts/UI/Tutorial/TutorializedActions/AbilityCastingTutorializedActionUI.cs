using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityCastingTutorializedActionUI : TutorializedActionUI
{
    [Header("Specific Components")]
    [SerializeField] private TextMeshProUGUI tutorializedActionText;

    private bool eventConditionMet = false;
    private AbilitySlot lastAbilitySlotUpgraded;

    protected override void OnEnable()
    {
        base.OnEnable();
        Ability.OnAnyAbilityCast += Ability_OnAnyAbilityCast;
        AbilityLevelHandler.OnAnyAbilityLevelSet += AbilityLevelHandler_OnAnyAbilityLevelSet;
        CentralizedInputSystemManager.OnRebindingCompleted += CentralizedInputSystemManager_OnRebindingCompleted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Ability.OnAnyAbilityCast -= Ability_OnAnyAbilityCast;
        AbilityLevelHandler.OnAnyAbilityLevelSet -= AbilityLevelHandler_OnAnyAbilityLevelSet;
        CentralizedInputSystemManager.OnRebindingCompleted -= CentralizedInputSystemManager_OnRebindingCompleted;
    }

    private void UpdateTutorializedActionText()
    {
        string abilityBinding;

        switch (lastAbilitySlotUpgraded)
        {
            case AbilitySlot.AbilityA:
                abilityBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.AbilityA));
                break;
            case AbilitySlot.AbilityB:
                abilityBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.AbilityB));
                break;
            case AbilitySlot.AbilityC:
                abilityBinding = MappingUtilities.MapLongBindingName(CentralizedInputSystemManager.Instance.GetBindingText(Binding.AbilityC));
                break;
            default:
                return;
        }

        tutorializedActionText.text = $"Prueba lanzar la habilidad aprendida presionando <b>{abilityBinding}</b>.";
    }

    #region Virtual Methods
    public override TutorializedAction GetTutorializedAction() => TutorializedAction.AbilityCasting;

    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;
        return eventConditionMet;
    }

    protected override void OpenTutorializedAction()
    {
        UpdateTutorializedActionText();
        base.OpenTutorializedAction();
    }
    #endregion

    #region Subscriptions
    private void Ability_OnAnyAbilityCast(object sender, Ability.OnAbilityCastEventArgs e)
    {
        eventConditionMet = true;
    }

    private void AbilityLevelHandler_OnAnyAbilityLevelSet(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        lastAbilitySlotUpgraded = e.ability.AbilitySlot;
    }

    private void CentralizedInputSystemManager_OnRebindingCompleted(object sender, CentralizedInputSystemManager.OnRebindingEventArgs e)
    {
        UpdateTutorializedActionText();
    }
    #endregion
}
