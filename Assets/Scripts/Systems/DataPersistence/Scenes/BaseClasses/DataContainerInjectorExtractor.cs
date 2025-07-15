using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataContainerInjectorExtractor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected SceneDataHandler sceneDataSaveLoader;

    public abstract void InjectAllDataFromDataContainers(); //Data Injection
    public abstract void ExtractAllDataToDataContainers(); //Data Extraction
}
