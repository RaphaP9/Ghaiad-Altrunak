using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunJSONDataContainerMiddleMan : JSONDataContainerMiddleMan<RunData>
{
    [Header("Data Containers")]
    [SerializeField] private RunDataContainer runDataContainer;

    public override void LoadDataToContainer(RunData data)
    {
        runDataContainer.SetRunData(data);
    }

    public override void SaveDataFromContainer(ref RunData data)
    {
        data = runDataContainer.RunData;
    }
}
