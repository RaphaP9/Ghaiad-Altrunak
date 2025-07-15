using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityInfoSO", menuName = "ScriptableObjects/Abilities/AbilityInfo")]
public class AbilityInfoSO : ScriptableObject
{
    public List<AbilityLevelInfo> abilityLevelInfos;

    public AbilityLevelInfo GetAbilityLevelInfoByLevel(AbilityLevel abilityLevel)
    {
        foreach(AbilityLevelInfo abilityLevelInfo in abilityLevelInfos)
        {
            if(abilityLevelInfo.abilityLevel == abilityLevel) return abilityLevelInfo;
        }

        Debug.Log($"Ability Level Info with Level: {abilityLevel} could not be found. Returning null.");

        return null;
    }
}

[System.Serializable]
public class AbilityLevelInfo
{
    public AbilityLevel abilityLevel;
    [TextArea(3,10)] public string levelDescription;
    [Space]
    [TextArea(3, 10)] public string cardLevelDescription;
    public Transform cardPrefab;
}
