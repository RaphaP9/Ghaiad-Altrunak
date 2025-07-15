using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

public class JSONUtilityDataServiceEncryption : IDataService
{
    private readonly string encryptionCodeWord = "iox77qao";

    private string dirPath;
    private string filePath;

    public JSONUtilityDataServiceEncryption(string dirPath, string filePath)
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

            string dataToStore = JsonUtility.ToJson(data, false);
            
            dataToStore = EncryptData(dataToStore);

            using FileStream stream = new FileStream(tempPath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(dataToStore);
            writer.Close();

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

            string dataToStore = JsonUtility.ToJson(data, false);

            dataToStore = EncryptData(dataToStore);

            using FileStream stream = new FileStream(tempPath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            await writer.WriteAsync(dataToStore);
            writer.Close();

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

        T loadedData = default;

        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";
                using FileStream stream = new FileStream(path, FileMode.Open);
                using StreamReader reader = new StreamReader(stream);
                dataToLoad = reader.ReadToEnd();
           
                dataToLoad = DecryptData(dataToLoad);
                
                loadedData = JsonUtility.FromJson<T>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                return default;
            }
        }

        return loadedData;
    }

    public async Task<T> LoadDataAsync<T>()
    {
        string path = Path.Combine(dirPath, filePath);

        T loadedData = default;

        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";
                using FileStream stream = new FileStream(path, FileMode.Open);
                using StreamReader reader = new StreamReader(stream);
                dataToLoad = await reader.ReadToEndAsync();

                dataToLoad = DecryptData(dataToLoad);

                loadedData = JsonUtility.FromJson<T>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                return default;
            }
        }

        return loadedData;
    }

    #endregion

    #region Encryption

    private string EncryptData(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }


    private string DecryptData(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        Debug.Log($"Decrypted result (if not legible, wrong encryption word): {modifiedData}");

        return modifiedData;
    }
    #endregion
}
