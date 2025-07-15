using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunJSONDataSaveLoader : JSONDataSaveLoader<RunData>
{
    [Header("Data Containers")]
    [SerializeField] private SessionRunDataContainer sessionRunDataContainer;

    public override void LoadData(RunData data)
    {
        sessionRunDataContainer.SetRunData(data);
    }

    public override void SaveData(ref RunData data)
    {
        data = sessionRunDataContainer.RunData;
    }
}
