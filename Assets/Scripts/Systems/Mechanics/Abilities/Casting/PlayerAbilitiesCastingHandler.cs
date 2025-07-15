using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesCastingHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AbilitySlotHandler> abilitySlotsHandlers;

    public void HandleAbilitiesCasting() //Called By PlayerStateHandler
    {
        foreach (AbilitySlotHandler abilitySlotHandler in abilitySlotsHandlers)
        {
            abilitySlotHandler.HandleAbilityCasting();
        }
    }
}
