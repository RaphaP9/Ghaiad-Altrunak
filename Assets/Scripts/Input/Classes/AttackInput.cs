using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackInput : MonoBehaviour
{
    public static AttackInput Instance { get; private set; }

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
    public abstract bool GetAttackDown();
    public abstract bool GetAttackHold();
}
