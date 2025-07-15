using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public interface IDataPersistenceManager 
{
    public void LoadData();
    public void SaveData();
    public Task LoadDataAsync();
    public Task SaveDataAsync();
}
