using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataSaveLoader<T>
{
    public void LoadDataToContainer(T data);
    public void SaveDataFromContainer(ref T data);
}
