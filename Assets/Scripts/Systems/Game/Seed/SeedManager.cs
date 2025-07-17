using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public static SeedManager Instance { get; private set; }

    [Header("RuntimeFilled")]
    [SerializeField] private string seed;

    public string Seed => seed;

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
            Debug.LogWarning("There is more than one SeedManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void SetSeed(string setterSeed) => seed = setterSeed;
}
