using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatsAssetLibrary : MonoBehaviour
{
    public static TreatsAssetLibrary Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<TreatSO> treats;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<TreatSO> Treats => treats;

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
            //Debug.LogWarning("There is more than one TreatsAssetLibrary instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public TreatSO GetTreatSOByID(int id)
    {
        foreach (TreatSO treatSO in treats)
        {
            if (treatSO.id == id) return treatSO;
        }

        if (debug) Debug.Log($"No TreatSO matches the ID:{id}. Returning null");
        return null;
    }

    public TreatSO GetTreatSOByName(string name)
    {
        foreach (TreatSO treatSO in treats)
        {
            if (treatSO._name == name) return treatSO;
        }

        if (debug) Debug.Log($"No TreatSO matches the Name:{name}. Returning null");
        return null;
    }
}
