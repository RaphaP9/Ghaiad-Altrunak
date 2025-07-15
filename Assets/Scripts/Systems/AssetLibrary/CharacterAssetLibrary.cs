using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAssetLibrary : MonoBehaviour
{
    public static CharacterAssetLibrary Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<CharacterSO> characters;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<CharacterSO> Characters => characters;
            
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
            //Debug.LogWarning("There is more than one CharacterAssetLibrary instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public CharacterSO GetCharacterSOByID(int id)
    {
        foreach (CharacterSO characterSO in characters)
        {
            if (characterSO.id == id) return characterSO;
        }

        if (debug) Debug.Log($"No CharacterSO matches the ID:{id}. Returning null");
        return null;
    }

    public CharacterSO GetCharacterSOByName(string name)
    {
        foreach (CharacterSO characterSO in characters)
        {
            if (characterSO.entityName == name) return characterSO;
        }

        if (debug) Debug.Log($"No CharacterSO matches the Name:{name}. Returning null");
        return null;
    }
}
