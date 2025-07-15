using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunJSONDataManager : JSONDataManager<RunData>
{
    [Header("Run Components")]
    [SerializeField] private RunJSONDataContainerMiddleMan runJSONDataContainerMiddleMan;

    public static RunJSONDataManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one RunDataPersistenceManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override JSONDataContainerMiddleMan<RunData> GetSONDataContainerMiddleMan() => runJSONDataContainerMiddleMan;
}
