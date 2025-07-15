using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalTestObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifespan;

    private void Start()
    {
        Destroy(gameObject, lifespan);
    }
}
