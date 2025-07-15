using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatResolver : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
        StatModifierManager.OnStatModifierManagerUpdated += StatModifierManager_OnStatModifierManagerUpdated;
    }

    protected virtual void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
        StatModifierManager.OnStatModifierManagerUpdated -= StatModifierManager_OnStatModifierManagerUpdated;
    }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    protected abstract void SetSingleton();

    protected abstract void InitializeResolver();
    protected abstract void OnResolverInitializedMethod();

    protected abstract void UpdateResolver();
    protected abstract void OnResolverUpdatedMethod();

    #region Subscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        InitializeResolver();
    }
    private void StatModifierManager_OnStatModifierManagerUpdated(object sender, System.EventArgs e)
    {
        UpdateResolver();
    }
    #endregion
}
