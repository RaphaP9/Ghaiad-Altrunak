using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/MusicPool")]
public class MusicPoolSO : ScriptableObject
{
    [Header("Scenes")]
    public AudioClip menuMusic;
    public AudioClip creditsMusic;

    [Header("Gameplay")]
    public List<CharacterStagesMusicGroup> CharacterStagesMusicGroupList;
}

[System.Serializable]
public class CharacterStagesMusicGroup
{
    public CharacterSO characterSO;
    public List<StageAudioClipGroup> stageAudioClipGroups;
}

[System.Serializable]
public class StageAudioClipGroup
{
    public int stageNumber;
    public AudioClip audioClip;
}
