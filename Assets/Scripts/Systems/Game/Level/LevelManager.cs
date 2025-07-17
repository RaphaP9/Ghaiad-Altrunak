using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("RuntimeFilled")]
    [SerializeField] private int currentLevel;

    public int CurrentLevel => currentLevel;

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
            Debug.LogWarning("There is more than one LevelManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void SetCurrentLevel(int setterLevel) => currentLevel = setterLevel;
}
