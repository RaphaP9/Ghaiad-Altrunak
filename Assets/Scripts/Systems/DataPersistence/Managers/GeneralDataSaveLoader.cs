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
    [SerializeField] private JSONPerpetualDataPersistenceManager JSONPerpetualDataPersistenceManager;
    [SerializeField] private JSONRunDataPersistenceManager JSONRunDataPersistenceManager;   

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

        JSONPerpetualDataPersistenceManager.LoadData(); //NOTE: Order is important
        JSONRunDataPersistenceManager.LoadData();

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }
    public async Task LoadAllJSONDataAsync()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);

        await JSONPerpetualDataPersistenceManager.LoadDataAsync();
        await JSONRunDataPersistenceManager.LoadDataAsync();

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Perpetual JSON Load
    public void PerpetualDataLoad()
    {
        LoadPerpetualJSONData();
        InjectAllDataFromContainers();
    }

    public async Task PerpetualDataLoadAsync()
    {
        await LoadPerpetualJSONDataAsync();
        InjectAllDataFromContainers();
    }

    public void LoadPerpetualJSONData()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);
        JSONPerpetualDataPersistenceManager.LoadData(); 
        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoadPerpetualJSONDataAsync()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);
        await JSONPerpetualDataPersistenceManager.LoadDataAsync();
        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Run JSON Load
    public void RunDataLoad()
    {
        LoadRunJSONData();
        InjectAllDataFromContainers();
    }

    public async Task RunDataLoadAsync()
    {
        await LoadRunJSONDataAsync();
        InjectAllDataFromContainers();
    }
    public void LoadRunJSONData()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);
        JSONRunDataPersistenceManager.LoadData();
        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoadRunJSONDataAsync()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);
        await JSONPerpetualDataPersistenceManager.LoadDataAsync();
        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Data Containers Injection
    public void InjectAllDataFromContainers()
    {
        List<SessionDataSaveLoader> sessionDataSaveLoaders = FindObjectsOfType<SessionDataSaveLoader>().ToList();

        foreach (SessionDataSaveLoader sessionDataSaveLoader in sessionDataSaveLoaders)
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

        JSONPerpetualDataPersistenceManager.SaveData();
        JSONRunDataPersistenceManager.SaveData();

        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SaveAllJSONDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);

        await JSONPerpetualDataPersistenceManager.SaveDataAsync();
        await JSONRunDataPersistenceManager.SaveDataAsync();

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
        JSONPerpetualDataPersistenceManager.SaveData();
        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SavePerpetualJSONDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);
        await JSONPerpetualDataPersistenceManager.SaveDataAsync();
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
        JSONRunDataPersistenceManager.SaveData();
        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SaveRunJSONDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);
        await JSONRunDataPersistenceManager.SaveDataAsync();
        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Data Containers Extraction
    public void ExtractAllDataToContainers()
    {
        List<SessionDataSaveLoader> sessionDataSaveLoaders = FindObjectsOfType<SessionDataSaveLoader>().ToList();

        foreach (SessionDataSaveLoader sessionDataSaveLoader in sessionDataSaveLoaders)
        {
            sessionDataSaveLoader.ExtractAllDataToDataContainers();
        }
    }
    #endregion
}
