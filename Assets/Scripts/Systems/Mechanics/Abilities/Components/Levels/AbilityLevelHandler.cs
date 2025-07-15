using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLevelHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilityIdentifier abilityIdentifier;

    [Header("Setting")]
    [SerializeField] private AbilityLevel maxLevel;
    [SerializeField] private AbilityLevel startingAbilityLevel;

    [Header("Runtime Filled")]
    [SerializeField] private AbilityLevel abilityLevel;

    public AbilitySO AbilitySO => abilityIdentifier.AbilitySO;
    public AbilityLevel AbilityLevel => abilityLevel;

    public event EventHandler<OnAbilityLevelEventArgs> OnAbilityLevelSet;
    public event EventHandler<OnAbilityLevelEventArgs> OnAbilityLevelInitialized;

    public static event EventHandler<OnAbilityLevelEventArgs> OnAnyAbilityLevelSet;
    public static event EventHandler<OnAbilityLevelEventArgs> OnAnyAbilityLevelInitialized;

    public class OnAbilityLevelEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
        public AbilityLevel abilityLevel;
        public Ability ability;
    }

    private void Start()
    {
        InitializeAbilityLevel(startingAbilityLevel);
    }

    public bool IsUnlocked() => abilityLevel != AbilityLevel.NotLearned;
    public bool IsMaxedOut() => abilityLevel == maxLevel;

    #region Ability Level Selection
    public void InitializeAbilityLevel(AbilityLevel abilityLevel)
    {
        this.abilityLevel = abilityLevel;
        OnAbilityLevelInitialized?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel, ability = abilityIdentifier.Ability });     
        OnAnyAbilityLevelInitialized?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel, ability = abilityIdentifier.Ability });
    }

    public void SetAbilityLevel(AbilityLevel abilityLevel)
    {
        this.abilityLevel = abilityLevel;
        OnAbilityLevelSet?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel, ability = abilityIdentifier.Ability });
        OnAnyAbilityLevelSet?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel, ability = abilityIdentifier.Ability });
    }
    #endregion

    #region Get & Set
    public void SetStartingAbilityLevel(AbilityLevel abilityLevel)
    {
        startingAbilityLevel = abilityLevel;
    }

    public PrimitiveAbilityLevelGroup GetPrimitiveAbilityLevelGroup()
    {
        PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup = new PrimitiveAbilityLevelGroup { abilitySO = AbilitySO, abilityLevel = abilityLevel };
        return primitiveAbilityLevelGroup;
    }
    #endregion
}
