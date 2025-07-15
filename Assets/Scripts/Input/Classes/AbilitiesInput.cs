using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitiesInput : MonoBehaviour
{
    public static AbilitiesInput Instance { get; private set; }

    protected virtual void Awake()
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

    public abstract bool CanProcessInput();

    public abstract bool GetAbilityADown();
    public abstract bool GetAbilityAHold();

    public abstract bool GetAbilityBDown();
    public abstract bool GetAbilityBHold();

    public abstract bool GetAbilityCDown();
    public abstract bool GetAbilityCHold();
}
