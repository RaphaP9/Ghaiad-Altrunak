using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHittableObjectSO", menuName = "ScriptableObjects/HittableObjects/HittableObject(Default)")]
public class HittableObjectSO : ScriptableObject, IAttackable
{
    [Header("Hittable Object Identifiers")]
    public int id;
    public string hittableObjectName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;
    [ColorUsage(true, true)] public Color color;
    [Space]
    public Transform prefab;

    [Header("Entity Stats")]
    [Range(0, 5)] public int health;
    [Range(0, 5)] public int shield;

    #region IAttackableSO Methods
    public string GetAttackableName() => hittableObjectName;
    public string GetAttackableDescription() => description;
    public Sprite GetAttackableSprite() => sprite;
    #endregion
}
