using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenInput : MonoBehaviour
{
    public static ScreenInput Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] protected Vector2 lastValidWorldMousePosition = Vector2.zero;

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

    public abstract Vector2 GetWorldMousePosition();
}
