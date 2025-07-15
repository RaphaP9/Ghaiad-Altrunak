using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalAssetStatModifierManager : AssetStatModifierManager
{
    public static TemporalAssetStatModifierManager Instance { get; private set; }

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
            Debug.LogWarning("There is more than one TemporalAssetStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
