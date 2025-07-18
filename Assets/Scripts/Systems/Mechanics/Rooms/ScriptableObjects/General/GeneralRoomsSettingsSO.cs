using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralRoomsSettingsSO", menuName = "ScriptableObjects/Rooms/GeneralRoomsSettings")]
public class GeneralRoomsSettingsSO : ScriptableObject
{
    [Header("Lists")]
    public List<LevelRoomSettingsSO> levelRoomSettingsList;

    [Header("Default")]
    public LevelRoomSettingsSO defaultLevelRoomSettings;

    public LevelRoomSettingsSO FindLevelSettingsByLevel(int level)
    {
        foreach(LevelRoomSettingsSO levelSetting in levelRoomSettingsList)
        {
            if(levelSetting.level == level) return levelSetting;
        }

        Debug.Log($"Could not find level setting for Level: {level}. Returning default setting");
        return defaultLevelRoomSettings;
    }
}
