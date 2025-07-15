using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PerpetualJSONDataContainerMiddleMan : JSONDataContainerMiddleMan<PerpetualData>
{
    [Header("Data Containers")]
    [SerializeField] private PerpetualDataContainer perpetualDataContainer;

    public override void LoadDataToContainer(PerpetualData data)
    {
        perpetualDataContainer.SetPerpetualData(data);
    }

    public override void SaveDataFromContainer(ref PerpetualData data)
    {
        data = perpetualDataContainer.PerpetualData;
    }
}
