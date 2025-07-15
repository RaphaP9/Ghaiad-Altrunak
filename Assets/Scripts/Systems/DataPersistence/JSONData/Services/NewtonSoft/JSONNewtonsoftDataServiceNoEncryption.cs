using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class JSONNewtonsoftDataServiceNoEncryption : IDataService
{
    private string dirPath;
    private string filePath;

    public JSONNewtonsoftDataServiceNoEncryption(string dirPath, string filePath)
    {
        this.dirPath = dirPath;
        this.filePath = filePath;
    }

    #region Save Data

    public bool SaveData<T>(T data)
    {
        string finalPath = Path.Combine(dirPath, filePath);
        string tempPath = finalPath + ".tmp";

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(finalPath));

            if (!File.Exists(finalPath))
            {
                //Debug.Log("Writing file for the first time!");
            }
            else
            {
                //Debug.Log("Data exists. Deleting old file and writing a new one!");
                //File.Delete(path);
            }

            FileStream stream = File.Create(tempPath);
            stream.Close();

            File.WriteAllText(tempPath, JsonConvert.SerializeObject(data, Formatting.Indented));

            if (File.Exists(finalPath)) File.Delete(finalPath);

            File.Move(tempPath, finalPath);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");

            if (File.Exists(tempPath)) File.Delete(tempPath);

            return false;
        }

    }

    public async Task<bool> SaveDataAsync<T>(T data)
    {
        string finalPath = Path.Combine(dirPath, filePath);
        string tempPath = finalPath + ".tmp";

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(finalPath));

            if (!File.Exists(finalPath))
            {
                //Debug.Log("Writing file for the first time!");
            }
            else
            {
                //Debug.Log("Data exists. Deleting old file and writing a new one!");
                //File.Delete(path);
            }

            FileStream stream = File.Create(tempPath);
            stream.Close();

            await File.WriteAllTextAsync(tempPath, JsonConvert.SerializeObject(data, Formatting.Indented));

            if (File.Exists(finalPath)) File.Delete(finalPath);

            File.Move(tempPath, finalPath);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");

            if (File.Exists(tempPath)) File.Delete(tempPath);

            return false;
        }

    }

    #endregion

    #region Load Data

    public T LoadData<T>()
    {
        string path = Path.Combine(dirPath, filePath);

        if (!File.Exists(path))
        {
            //Debug.LogError($"Cannot load file at {path}: File does not exist!");
            //throw new FileNotFoundException($"{path} does not exists");

            return default;
        }

        try
        {
            T data;
            data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            return default;
        }
    }

    public async Task<T> LoadDataAsync<T>()
    {
        string path = Path.Combine(dirPath, filePath);

        if (!File.Exists(path))
        {
            return default;
        }

        try
        {
            T data;
            data = JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(path));

            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            return default;
        }
    }
    #endregion
}
