using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsAssetLibrary : MonoBehaviour
{
    public static ObjectsAssetLibrary Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<ObjectSO> objects;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<ObjectSO> Objects => objects;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.LogWarning("There is more than one ObjectsAssetLibrary instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public ObjectSO GetObjectSOByID(int id)
    {
        foreach (ObjectSO objectSO in objects)
        {
            if (objectSO.id == id) return objectSO;
        }

        if (debug) Debug.Log($"No ObjectSO matches the ID:{id}. Returning null");
        return null;
    }

    public ObjectSO GetObjectSOByName(string name)
    {
        foreach (ObjectSO objectSO in objects)
        {
            if (objectSO._name == name) return objectSO;
        }

        if (debug) Debug.Log($"No ObjectSO matches the Name:{name}. Returning null");
        return null;
    }
}
