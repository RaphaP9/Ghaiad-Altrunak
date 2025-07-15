using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualJSONDataManager : JSONDataManager<PerpetualData>
{
    [Header("Perpetual Components")]
    [SerializeField] private PerpetualJSONDataContainerMiddleMan perpetualJSONDataContainerMiddleMan;

    public static PerpetualJSONDataManager Instance { get; private set; }

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

    protected override JSONDataContainerMiddleMan<PerpetualData> GetSONDataContainerMiddleMan() => perpetualJSONDataContainerMiddleMan;
}