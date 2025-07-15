using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementInput : MonoBehaviour
{
    public static MovementInput Instance { get; private set; }

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
    public abstract Vector2 GetMovementInputNormalized();
    public abstract Vector2 GetLastNonZeroMovementInputNormalized();
}
