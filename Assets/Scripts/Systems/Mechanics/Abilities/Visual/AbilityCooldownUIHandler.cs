using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityCooldownUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform cooldownPanelTransform;

    [Header("Components")]
    [SerializeField] private CooldownCounterUIHandler cooldownCounterUIHandler;

    [Header("Runtime Filled")]
    [SerializeField] private Ability ability;
    [SerializeField] private AbilityCooldownHandler abilityCooldownHandler;

    #region Flags
    private bool UIEnabled = false;
    #endregion

    private void Awake()
    {
        InitializeUI();
    }

    private void Update()
    {
        HandleCooldownUI();
    }

    private void InitializeUI()
    {
        DisableCooldownUI();
        UIEnabled = false;
    }

    private void HandleCooldownUI()
    {
        if (abilityCooldownHandler == null) return;

        //NOTE: CooldownCounterUIHandler does not update the cooldownText every frame (See CooldownCounterUIHandler class)
        if(abilityCooldownHandler.CooldownTimer > 0)
        {
            cooldownCounterUIHandler.UpdateCooldownValues(abilityCooldownHandler.CooldownTimer); 
        }

        if(abilityCooldownHandler.CooldownTimer > 0 && !UIEnabled)
        {
            EnableCooldownUI();
            UIEnabled = true;
        }

        if (abilityCooldownHandler.CooldownTimer <= 0 && UIEnabled)
        {
            DisableCooldownUI();
            cooldownCounterUIHandler.ClearCooldownValues();
            UIEnabled = false;
        }
    }

    #region Public Methods
    public void AssignAbility(Ability ability)
    {
        SetAbility(ability);

        switch (ability.AbilitySO.GetAbilityType())
        {
            case AbilityType.Passive:
                ClearAbilityCooldownHandler();
                break;
            case AbilityType.Active:
                SetAbilityCooldownHandler((ability as ActiveAbility).AbilityCooldownHandler);
                break;
            case AbilityType.ActivePassive:
                SetAbilityCooldownHandler((ability as ActivePassiveAbility).AbilityCooldownHandler);
                break;
        }
    }
    #endregion

    private void EnableCooldownUI() => cooldownPanelTransform.gameObject.SetActive(true);
    private void DisableCooldownUI() => cooldownPanelTransform.gameObject.SetActive(false);

    #region Setters
    private void SetAbility(Ability ability) => this.ability = ability;
    private void ClearAbility() => ability = null;
    private void SetAbilityCooldownHandler(AbilityCooldownHandler abilityCooldownHandler) => this.abilityCooldownHandler = abilityCooldownHandler;
    private void ClearAbilityCooldownHandler() => abilityCooldownHandler = null;
    #endregion
}
