using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SceneDataHandler : MonoBehaviour
{
    public static SceneDataHandler Instance {  get; private set; }

    [Header("Settings")]
    [SerializeField] private LoadMode awakeLoadMode;
    [SerializeField] private SaveMode applicationQuitSaveMode; //Only Unity Editor

    //CompleteDataLoad/Save performs both JSON and session data operations
    private enum LoadMode {CompleteDataLoad, InjectionFromContainers, NoLoad}
    private enum SaveMode {CompleteDataSave, ExtractionToContainers, NoSave}

    protected virtual void Awake()
    {
        SetSingleton();
        HandleDataLoadOnAwake();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandleDataLoadOnAwake() //Synchronous Methods!
    {
        switch (awakeLoadMode)
        {
            case LoadMode.CompleteDataLoad:
                GeneralDataManager.Instance.CompleteDataLoad(); // Loads All JSON Data and Injects To Containers
                break;
            case LoadMode.InjectionFromContainers:
            default:
                GeneralDataManager.Instance.InjectAllDataFromContainers();
                break;
            case LoadMode.NoLoad:
                break;
        }
    }

    private void HandleDataSaveOnQuit() //Synchronous Methods!
    {
        switch (applicationQuitSaveMode)
        {
            case SaveMode.CompleteDataSave:
                GeneralDataManager.Instance.CompleteDataSave(); // Extracts All Data To Containers and Saves ALL JSON Data
                break;
            case SaveMode.ExtractionToContainers:
            default:
                GeneralDataManager.Instance.ExtractAllDataToContainers();
                break;
            case SaveMode.NoSave:
                break;
        }
    }

    private void OnApplicationQuit()
    {
        HandleDataSaveOnQuit();
    }
}
