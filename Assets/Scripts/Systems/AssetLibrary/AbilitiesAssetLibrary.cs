using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AbilitiesAssetLibrary : MonoBehaviour
{
    public static AbilitiesAssetLibrary Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<AbilitySO> abilities;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilitySO> Abilities => abilities;


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
            //Debug.LogWarning("There is more than one AbilitiesAssetLibrary instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public AbilitySO GetAbilitySOByID(int id)
    {
        foreach (AbilitySO abilitySO in abilities)
        {
            if (abilitySO.id == id) return abilitySO;
        }

        if (debug) Debug.Log($"No AbilitySO matches the ID: {id}. Returning null");
        return null;
    }

    public AbilitySO GetAbilitySOByName(string name)
    {
        foreach (AbilitySO abilitySO in abilities)
        {
            if (abilitySO.abilityName == name) return abilitySO;
        }

        if (debug) Debug.Log($"No AbilitySO matches the Name: {name}. Returning null");
        return null;
    }
}
