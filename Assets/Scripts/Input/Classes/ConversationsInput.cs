using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConversationsInput : MonoBehaviour
{
    public static ConversationsInput Instance { get; private set; }

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
    public abstract bool GetSkipDown();
}
