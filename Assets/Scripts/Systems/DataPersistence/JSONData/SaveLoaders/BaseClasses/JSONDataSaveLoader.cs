using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JSONDataSaveLoader<T> : MonoBehaviour, IDataSaveLoader<T>
{
    public static event EventHandler OnSessionDataLoaded;
    public static event EventHandler OnSessionDataSaved;

    protected virtual void OnDataLoadedMethod()
    {
        OnSessionDataLoaded?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnDataSavedMethod()
    {
        OnSessionDataSaved?.Invoke(this, EventArgs.Empty);
    }

    public abstract void LoadData(T data);
    public abstract void SaveData(ref T data);
}
