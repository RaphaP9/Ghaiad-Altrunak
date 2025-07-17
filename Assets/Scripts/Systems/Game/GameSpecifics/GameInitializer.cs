using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    //Note: GameInitializer has a ScriptExecutionOrder before default time

    public static GameInitializer Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameInitializer instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        PlayerCharacterManager.Instance.InstantiatePlayer();
    }
}

