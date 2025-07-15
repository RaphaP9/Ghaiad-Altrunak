using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityKeyBindUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform keyBindUITransform;
    [SerializeField] private TextMeshProUGUI keyBindText;

    [Header("Runtime Filled")]
    [SerializeField] private Ability ability;
    [SerializeField] private AbilitySlot abilitySlot;

    private bool firstUpdateLogicPerformed = false;

    private void OnEnable()
    {
        AbilityLevelHandler.OnAnyAbilityLevelInitialized += AbilityLevelHandler_OnAnyAbilityLevelInitialized;
        AbilityLevelHandler.OnAnyAbilityLevelSet += AbilityLevelHandler_OnAnyAbilityLevelSet;

        CentralizedInputSystemManager.OnRebindingCompleted += CentralizedInputSystemManager_OnRebindingCompleted;
    }

    private void OnDisable()
    {
        AbilityLevelHandler.OnAnyAbilityLevelInitialized -= AbilityLevelHandler_OnAnyAbilityLevelInitialized;
        AbilityLevelHandler.OnAnyAbilityLevelSet -= AbilityLevelHandler_OnAnyAbilityLevelSet;

        CentralizedInputSystemManager.OnRebindingCompleted -= CentralizedInputSystemManager_OnRebindingCompleted;
    }

    private void Start()
    {
        UpdateKeyBindText();
    }

    private void Update()
    {
        HandleFirstUpdateLogic();
    }

    private void HandleFirstUpdateLogic() //Performed in first update to make sure AbilityLevelHandler has already initialized the AbilityLevel (Initializes on Start())
    {
        if (firstUpdateLogicPerformed) return;

        firstUpdateLogicPerformed = true;

        if (ability == null) return;
        HandleKeyBindUIShowing(ability.AbilityLevel);
    }

    private void HandleKeyBindUIShowing(AbilityLevel abilityLevel)
    {
        if(abilitySlot == AbilitySlot.Passive)
        {
            DisableKeyBindUI();
            return;
        }

        if (abilityLevel == AbilityLevel.NotLearned)
        {
            DisableKeyBindUI();
        }
        else
        {
            EnableKeyBindUI();
        }
    }

    #region Public Methods
    public void AssignAbility(Ability ability)
    {
        SetAbility(ability);

        if (!firstUpdateLogicPerformed) return;
        HandleKeyBindUIShowing(ability.AbilityLevel);
    }
    #endregion

    #region Enable & Disable
    private void EnableKeyBindUI() => keyBindUITransform.gameObject.SetActive(true);
    private void DisableKeyBindUI() => keyBindUITransform.gameObject.SetActive(false);
    #endregion

    #region Setters
    private void SetAbility(Ability ability) => this.ability = ability;
    private void ClearAbility() => ability = null;

    public void SetAbilitySlot(AbilitySlot abilitySlot) => this.abilitySlot = abilitySlot;
    #endregion

    #region Update KeyBind Text
    private void UpdateKeyBindText()
    {
        switch (abilitySlot)
        {
            case AbilitySlot.AbilityA:
                SetKeyBindText(CentralizedInputSystemManager.Instance.GetBindingText(Binding.AbilityA));
                break;
            case AbilitySlot.AbilityB:
                SetKeyBindText(CentralizedInputSystemManager.Instance.GetBindingText(Binding.AbilityB));
                break;
            case AbilitySlot.AbilityC:
                SetKeyBindText(CentralizedInputSystemManager.Instance.GetBindingText(Binding.AbilityC));
                break;
        }
    }

    private void SetKeyBindText(string text) => keyBindText.text = text;
    #endregion

    #region Subscriptions
    private void AbilityLevelHandler_OnAnyAbilityLevelInitialized(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        if (ability?.AbilitySO != e.abilitySO) return;

        HandleKeyBindUIShowing(e.abilityLevel);
    }

    private void AbilityLevelHandler_OnAnyAbilityLevelSet(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        if (ability?.AbilitySO != e.abilitySO) return;

        HandleKeyBindUIShowing(e.abilityLevel);
    }

    private void CentralizedInputSystemManager_OnRebindingCompleted(object sender, CentralizedInputSystemManager.OnRebindingEventArgs e)
    {
        UpdateKeyBindText();
    }

    #endregion
}
