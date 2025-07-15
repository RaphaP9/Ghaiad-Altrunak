using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLibrary : MonoBehaviour
{
    public static AssetLibrary Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.LogWarning("There is more than one AssetLibrary instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
