using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreatEffectSO : ScriptableObject
{
    [Header("Activator Inventory Objects")]
    public List<InventoryObjectSO> activatorInventoryObjects;

    [Header("Settings")]
    public string treatName;
    public Sprite sprite;
    [TextArea(3, 10)] public string description;
}
