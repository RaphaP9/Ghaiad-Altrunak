using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MusicPoolSO musicPoolSO;

    [Header("Gameplay Music Change Settings")]
    [SerializeField,Range(0.1f, 2f)]  private float fadeOutTime;
    [SerializeField, Range(0.1f, 2f)] private float fadeInTime;

    [Header("Cinematic Gameplay Music Fade Settings")]
    [SerializeField, Range(0.1f, 2f)] private float fadeOutTimeCinematics;
    [SerializeField, Range(0.1f, 2f)] private float fadeInTimeCinematics;

    [Header("Debug")]
    [SerializeField] private AudioClip currentGameplayMusic;

    private void OnEnable()
    {
        GeneralStagesManager.OnStageInitialized += GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange += GeneralStagesManager_OnStageChange;

        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GeneralStagesManager.OnStageInitialized -= GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange -= GeneralStagesManager_OnStageChange;

        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void PlayGameplayMusic(int stageNumber)
    {
        AudioClip musicToPlay = GetGameplayMusicToPlay(stageNumber);
        PlayGameplayMusic(musicToPlay);
    }

    private AudioClip GetGameplayMusicToPlay(int stageNumber)
    {
        CharacterSO characterSO = PlayerCharacterManager.Instance.CharacterSO;
        CharacterStagesMusicGroup characterStagesMusicGroup = GetCharacterStagesMusicGroup(characterSO);

        if(characterStagesMusicGroup == null)
        {
            Debug.Log($"Can not find CharacterStagesMusicGroup for Character: {characterSO.entityName}");
            return null;
        }

        StageAudioClipGroup stageAudioClipGroup = GetStageAudioClipGroup(characterStagesMusicGroup, stageNumber);

        if (stageAudioClipGroup == null)
        {
            Debug.Log($"Can not find StageAudioClipGroup for Stage: {stageNumber}");
            return null;
        }

        return stageAudioClipGroup.audioClip;
    }

    #region Utility Methods
    private CharacterStagesMusicGroup GetCharacterStagesMusicGroup(CharacterSO characterSO)
    {
        foreach(CharacterStagesMusicGroup characterStagesMusicGroup in musicPoolSO.CharacterStagesMusicGroupList)
        {
            if(characterStagesMusicGroup.characterSO == characterSO) return characterStagesMusicGroup;
        }

        return null;
    }

    private StageAudioClipGroup GetStageAudioClipGroup(CharacterStagesMusicGroup characterStagesMusicGroup, int stageNumber)
    {
        foreach(StageAudioClipGroup stageAudioClipGroup in characterStagesMusicGroup.stageAudioClipGroups)
        {
            if(stageAudioClipGroup.stageNumber == stageNumber) return stageAudioClipGroup;
        }

        return null;
    }
    #endregion

    private void PlayGameplayMusic(AudioClip gameplayMusic)
    {
        MusicManager.Instance.PlayMusic(gameplayMusic);
        currentGameplayMusic = gameplayMusic;

        Debug.Log($"GameplayMusicPlay: {gameplayMusic}");
    }

    private IEnumerator FadeMusicOutCoroutine()
    {
        yield return StartCoroutine(MusicVolumeFadeManager.Instance.FadeOutVolumeCoroutine(fadeOutTime));
    }

    private IEnumerator FadeMusicInCoroutine()
    {
        yield return StartCoroutine(MusicVolumeFadeManager.Instance.FadeInVolumeCoroutine(fadeInTime));
    }

    #region Subscriptions
    private void GeneralStagesManager_OnStageInitialized(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        PlayGameplayMusic(e.stageNumber);
    }

    private void GeneralStagesManager_OnStageChange(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        PlayGameplayMusic(e.stageNumber);
    }

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.newState == GameManager.State.BeginningChangingStage)
        {
            StartCoroutine(FadeMusicOutCoroutine());
            return;
        }

        if (e.newState == GameManager.State.EndingChangingStage)
        {
            StartCoroutine(FadeMusicInCoroutine()); 
            return;
        }
    }
    #endregion
}
