using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class GeneralDataSaveLoader : MonoBehaviour
{
    public static GeneralDataSaveLoader Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private PerpetualJSONDataManager perpetualJSONDataManager;
    [SerializeField] private RunJSONDataManager runJSONDataManager;   

    public static event EventHandler OnDataLoadStart;
    public static event EventHandler OnDataLoadComplete;

    public static event EventHandler OnDataSaveStart;
    public static event EventHandler OnDataSaveComplete;    

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one GeneralDataSaveLoader instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Complete JSON Load
    public void CompleteDataLoad()
    {
        LoadAllJSONData();
        InjectAllDataFromContainers();
    }

    public async Task CompleteDataLoadAsync()
    {
        await LoadAllJSONDataAsync();
        InjectAllDataFromContainers();
    }

    public void LoadAllJSONData()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);

        perpetualJSONDataManager.LoadData(); //NOTE: Order is important
        runJSONDataManager.LoadData();

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }
    public async Task LoadAllJSONDataAsync()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);

        await perpetualJSONDataManager.LoadDataAsync();
        await runJSONDataManager.LoadDataAsync();

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Data Containers Injection
    public void InjectAllDataFromContainers()
    {
        List<DataContainerInjectorExtractor> sessionDataSaveLoaders = FindObjectsOfType<DataContainerInjectorExtractor>().ToList();

        foreach (DataContainerInjectorExtractor sessionDataSaveLoader in sessionDataSaveLoaders)
        {
            sessionDataSaveLoader.InjectAllDataFromDataContainers();
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////

    #region Complete JSON Save
    public void CompleteDataSave()
    {
        ExtractAllDataToContainers();
        SaveAllJSONData();
    }

    public async Task CompleteDataSaveAsync()
    {
        ExtractAllDataToContainers();
        await SaveAllJSONDataAsync();
    }

    public void SaveAllJSONData()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);

        perpetualJSONDataManager.SaveData();
        runJSONDataManager.SaveData();

        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SaveAllJSONDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);

        await perpetualJSONDataManager.SaveDataAsync();
        await runJSONDataManager.SaveDataAsync();

        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Perpetual JSON Save
    public void PerpetualDataSave()
    {
        ExtractAllDataToContainers();
        SavePerpetualJSONData();
    }

    public async Task PerpetualDataSaveAsync()
    {
        ExtractAllDataToContainers();
        await SavePerpetualJSONDataAsync();
    }

    public void SavePerpetualJSONData()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);
        perpetualJSONDataManager.SaveData();
        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SavePerpetualJSONDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);
        await perpetualJSONDataManager.SaveDataAsync();
        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Run JSON Save
    public void RunDataSave()
    {
        ExtractAllDataToContainers();
        SaveRunJSONData();
    }

    public async Task RunDataSaveAsync()
    {
        ExtractAllDataToContainers();
        await SaveRunJSONDataAsync();
    }

    public void SaveRunJSONData()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);
        runJSONDataManager.SaveData();
        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SaveRunJSONDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);
        await runJSONDataManager.SaveDataAsync();
        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Data Containers Extraction
    public void ExtractAllDataToContainers()
    {
        List<DataContainerInjectorExtractor> sessionDataSaveLoaders = FindObjectsOfType<DataContainerInjectorExtractor>().ToList();

        foreach (DataContainerInjectorExtractor sessionDataSaveLoader in sessionDataSaveLoaders)
        {
            sessionDataSaveLoader.ExtractAllDataToDataContainers();
        }
    }
    #endregion
}
