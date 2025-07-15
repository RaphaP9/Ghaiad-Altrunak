using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private MusicPoolSO musicPoolSO;

    [Header("Debug")]
    [SerializeField] private AudioClip currentMusic;

    private AudioSource audioSource;

    private const string MENU_SCENE_NAME = "MainMenu";
    private const string CREDITS_SCENE_NAME = "Credits";
    private const string GAMEPLAY_SCENE_NAME = "Gameplay";

    private void OnEnable()
    {
        ScenesManager.OnSceneLoadComplete += ScenesManager_OnSceneLoad;
    }

    private void OnDisable()
    {
        ScenesManager.OnSceneLoadComplete -= ScenesManager_OnSceneLoad;
    }

    private void Awake()
    {
        SetSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.LogWarning("There is more than one MusicManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        if (music == currentMusic) return;

        if (!music)
        {
            StopMusic();
        }

        if (audioSource.clip != music)
        {
            audioSource.Stop();
            audioSource.clip = music;
            audioSource.Play();
        }

        currentMusic = audioSource.clip;
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
        currentMusic = null;
    }

    private void HandleScenesMusicPlay(string sceneName)
    {
        switch (sceneName)
        {
            case GAMEPLAY_SCENE_NAME:
                //Handled By GameplayMusicManager;
                break;
            case MENU_SCENE_NAME:
                PlayMusic(musicPoolSO.menuMusic);
                Debug.Log("MainMenuMusicPlay");
                break;
            case CREDITS_SCENE_NAME:
                PlayMusic(musicPoolSO.creditsMusic);
                Debug.Log("CreditsMusicPlay");
                break;
            default:
                StopMusic();
                Debug.Log("Music Stopped CinematicScene");
            break;
        }
    }

    private void ScenesManager_OnSceneLoad(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        HandleScenesMusicPlay(e.targetScene);
    }
}
