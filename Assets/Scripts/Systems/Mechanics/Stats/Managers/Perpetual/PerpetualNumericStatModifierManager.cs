using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualNumericStatModifierManager : NumericStatModifierManager
{
    public static PerpetualNumericStatModifierManager Instance { get; private set; }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PerpetualNumericStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
