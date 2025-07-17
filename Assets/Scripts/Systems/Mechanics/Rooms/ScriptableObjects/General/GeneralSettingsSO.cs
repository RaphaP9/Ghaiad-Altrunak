using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralSettingsSO", menuName = "ScriptableObjects/Game/GeneralSettings")]
public class GeneralSettingsSO : ScriptableObject
{
    [Header("Lists")]
    public List<LevelSettingsSO> levelSettingsList;

    [Header("Default")]
    public LevelSettingsSO defaultLevelSettings;

    public LevelSettingsSO FindLevelSettingsByLevel(int level)
    {
        foreach(LevelSettingsSO levelSetting in levelSettingsList)
        {
            if(levelSetting.level == level) return levelSetting;
        }

        Debug.Log($"Could not find level setting for Level: {level}. Returning default setting");
        return defaultLevelSettings;
    }
}
