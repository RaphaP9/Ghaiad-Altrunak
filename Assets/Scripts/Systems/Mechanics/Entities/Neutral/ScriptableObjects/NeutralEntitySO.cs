using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNeutralEntitySO", menuName = "ScriptableObjects/Entities/NeutralEntities/NeutralEntity(Default)")]
public class NeutralEntitySO : EntitySO, IGoldSource
{
    [Header("NeutralEntity Extra Settings")]
    [Range(0, 10)] public int goldDrop;
    [Space]
    [Range(1f, 5f)] public float spawnDuration;
    [Range(1f, 10f)] public float cleanupTime;

    #region IGoldSource Methods
    public string GetGoldSourceName() => entityName;
    public string GetGoldSourceDescription() => description;
    public Sprite GetGoldSourceSprite() => sprite;
    #endregion

    public override DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.NeutralEntity;
}
