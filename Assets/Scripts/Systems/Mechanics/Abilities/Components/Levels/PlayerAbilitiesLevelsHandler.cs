using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesLevelsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AbilityLevelHandler> abilityLevelHandlers;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilityLevelHandler> AbilityLevelHandlers => abilityLevelHandlers;

    #region Set & Get

    public void SetStartingAbilityLevels(List<PrimitiveAbilityLevelGroup> setterPrimitiveAbilityLevelGroups) //To be called By the GameplaySessionDataSaveLoader, before Start()
    {
        foreach(PrimitiveAbilityLevelGroup setterPrimitiveAbilityLevelGroup in setterPrimitiveAbilityLevelGroups)
        {
            foreach(AbilityLevelHandler abilityLevelHandler in abilityLevelHandlers)
            {
                if (abilityLevelHandler.AbilitySO != setterPrimitiveAbilityLevelGroup.abilitySO) continue;

                abilityLevelHandler.SetStartingAbilityLevel(setterPrimitiveAbilityLevelGroup.abilityLevel);
            }
        }
    }

    public List<PrimitiveAbilityLevelGroup> GetPrimitiveAbilityLevelGroups()
    {
        List<PrimitiveAbilityLevelGroup> primitiveAbilityLevelGroups = new List<PrimitiveAbilityLevelGroup>();

        foreach (AbilityLevelHandler abilityLevelHandler in abilityLevelHandlers)
        {
            primitiveAbilityLevelGroups.Add(abilityLevelHandler.GetPrimitiveAbilityLevelGroup());
        }

        return primitiveAbilityLevelGroups;
    }
    #endregion
}
