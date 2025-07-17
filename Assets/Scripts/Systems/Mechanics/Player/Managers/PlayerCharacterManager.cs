using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
    public static PlayerCharacterManager Instance {  get; private set; }

    [Header("Default Settings")]
    [SerializeField] private CharacterSO defaultCharacterSO;
    [SerializeField] private Vector2Int defaultPosition;

    [Header("Settings - Runtime Filled")]
    [SerializeField] private CharacterSO characterSO;
    [SerializeField] private Vector2Int position;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public CharacterSO CharacterSO => characterSO;
    public Vector2Int Position => position;

    public static event EventHandler<OnPlayerCharacterEventArgs> OnPlayerCharacterSet;
    public static event EventHandler<OnPlayerCharacterEventArgs> OnPlayerCharacterSetPreInstantiation; //Only if character was null

    public class OnPlayerCharacterEventArgs : EventArgs
    {
        public CharacterSO characterSO;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Called By GameInitializer
    public void InstantiatePlayer()
    {
        if(characterSO == null)
        {
            if (debug) Debug.Log("CharacterSO is null on instantiation. Setting Character as Default Character to proceed instantiation.");
            characterSO = defaultCharacterSO;

        }

        OnPlayerCharacterSetPreInstantiation?.Invoke(this, new OnPlayerCharacterEventArgs { characterSO = characterSO });

        Transform instantiatedCharacter = Instantiate(characterSO.prefab, GeneralUtilities.Vector2IntToVector3(position), Quaternion.identity); 
    }

    public void SetCharacterSO(CharacterSO setterCharacterSO)
    {
        if(setterCharacterSO == null)
        {
            characterSO = defaultCharacterSO;
            if (debug) Debug.Log("CharacterSO is null. Setting Default Character.");
        }
        else
        {
            characterSO = setterCharacterSO;
            if (debug) Debug.Log($"CharacterSO set as: {characterSO.name}");
        }

        OnPlayerCharacterSet?.Invoke(this, new OnPlayerCharacterEventArgs { characterSO = characterSO });
    }

    public void SetPosition(Vector2Int setterPosition) => position = setterPosition;
}
