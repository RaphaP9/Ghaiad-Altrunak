using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitySlotsVariantsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AbilitySlotHandler> abilitySlotHandlers;

    public List<AbilitySlotHandler> AbilitySlotHandlers => abilitySlotHandlers;


    #region Set & Get

    public void SetStartingAbilityVariants(List<PrimitiveAbilitySlotGroup> setterPrimitiveAbilitySlotGroups) //To be called By the GameplaySessionDataSaveLoader, before Start()
    {
        foreach (PrimitiveAbilitySlotGroup setterPrimitiveAbilitySlotGroup in setterPrimitiveAbilitySlotGroups)
        {
            foreach (AbilitySlotHandler abilitySlotHandler in abilitySlotHandlers)
            {
                if (abilitySlotHandler.AbilitySlot != setterPrimitiveAbilitySlotGroup.abilitySlot) continue;

                abilitySlotHandler.SetStartingAbilityVariant(setterPrimitiveAbilitySlotGroup);
            }
        }
    }

    public List<PrimitiveAbilitySlotGroup> GetPrimitiveAbilitySlotGroups()
    {
        List<PrimitiveAbilitySlotGroup> primitiveAbilitySlotGroups = new List<PrimitiveAbilitySlotGroup>();

        foreach (AbilitySlotHandler abilitySlotHandler in abilitySlotHandlers)
        {
            primitiveAbilitySlotGroups.Add(abilitySlotHandler.GetPrimitiveAbilitySlotGroup());
        }

        return primitiveAbilitySlotGroups;
    }
    #endregion
}
