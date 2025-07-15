using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterAbilitySlotsRegister : MonoBehaviour
{
    public static CharacterAbilitySlotsRegister Instance { get; private set; }

    [Header("Lists - Runtime Filled")]
    [SerializeField] private List<AbilitySlotHandler> abilitySlotHandlers;

    public List<AbilitySlotHandler> AbilitySlotHandlers => abilitySlotHandlers;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PerformPlayerTransformAbilitiesSearch(Transform playerTransform)
    {
        ClearRegisteredAbilitySlotsList();

        AbilitySlotHandler[] abilitySlotHandlers = playerTransform.GetComponentsInChildren<AbilitySlotHandler>();
        SetRegisteredAbilitySlotsList(abilitySlotHandlers.ToList());
    }

    private void SetRegisteredAbilitySlotsList(List<AbilitySlotHandler> abilitySlotHandlers) => this.abilitySlotHandlers = abilitySlotHandlers;
    private void ClearRegisteredAbilitySlotsList() => abilitySlotHandlers.Clear();

    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        PerformPlayerTransformAbilitiesSearch(e.playerTransform);
    }
}
