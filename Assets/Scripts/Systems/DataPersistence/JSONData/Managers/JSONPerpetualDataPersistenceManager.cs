using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONPerpetualDataPersistenceManager : JSONDataPersistenceManager<PerpetualData>
{
    public static JSONPerpetualDataPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one GameDataPersistenceManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

}