using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isEnabled;

    public bool IsEnabled => isEnabled;

    public void SetIsEnabled(bool isValid) => this.isEnabled = isValid;
}
