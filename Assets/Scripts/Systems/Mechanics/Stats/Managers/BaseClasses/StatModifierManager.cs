using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatModifierManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] protected bool debug;

    public static event EventHandler OnStatModifierManagerInitialized;
    public static event EventHandler OnStatModifierManagerUpdated;

    protected void Awake()
    {
        SetSingleton();
    }

    protected virtual void Start()
    {
        InitializeManager();
    }

    protected abstract void SetSingleton();
    public abstract bool HasStatModifiers();
    public abstract int GetStatModifiersQuantity();

    protected virtual void InitializeManager()
    {
        OnStatModifierManagerInitialized?.Invoke(this, new EventArgs());    
    }

    protected virtual void UpdateStats()
    {
        OnStatModifierManagerUpdated?.Invoke(this, new EventArgs());
    }

    protected abstract StatValueType GetStatValueType();
    public abstract void RemoveStatModifiersByGUID(string originGUID);

    #region Subscriptions

    #endregion
}
