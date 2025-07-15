using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataSaveLoader<T>
{
    public void LoadData(T data);
    public void SaveData(ref T data);
}
