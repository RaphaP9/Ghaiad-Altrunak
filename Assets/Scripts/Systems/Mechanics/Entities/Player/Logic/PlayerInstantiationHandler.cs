using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantiationHandler : MonoBehaviour
{
    public static event EventHandler<OnPlayerInstantiationEventArgs> OnPlayerInstantiation;

    public class OnPlayerInstantiationEventArgs : EventArgs
    {
        public Transform playerTransform;
    }

    private void Awake()
    {
        OnPlayerInstantiation?.Invoke(this, new OnPlayerInstantiationEventArgs { playerTransform = transform});
    }
}
