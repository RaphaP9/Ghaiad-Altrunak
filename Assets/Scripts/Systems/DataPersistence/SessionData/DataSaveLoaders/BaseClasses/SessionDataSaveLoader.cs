using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SessionDataSaveLoader : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected SceneDataSaveLoader sceneDataSaveLoader;

    public abstract void InjectAllDataFromDataContainers(); //Data Injection
    public abstract void ExtractAllDataToDataContainers(); //Data Extraction
}
