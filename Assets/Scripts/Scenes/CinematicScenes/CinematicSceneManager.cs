using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicSceneManager : MonoBehaviour
{
    public static CinematicSceneManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Settings")]
    [SerializeField] private TransitionType transitionType;
    [SerializeField] private string nextScene;

    private const float SCENE_FADE_OUT_TIME = 0.5f;
    private const float MIN_SECURE_CINEMATIC_DURATION = 1f;

    private bool shouldSkipCinematic = false;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        StartCoroutine(CinematicCoroutine());
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CinematicSceneManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private IEnumerator CinematicCoroutine()
    {
        float clipTotalDuration = videoPlayer.frameCount / (float)videoPlayer.frameRate;
        float clipFixedDuration = clipTotalDuration - SCENE_FADE_OUT_TIME;

        clipFixedDuration = clipFixedDuration < MIN_SECURE_CINEMATIC_DURATION ? MIN_SECURE_CINEMATIC_DURATION : clipFixedDuration;

        float elapsedTime = 0;

        while (elapsedTime <= clipFixedDuration)
        {
            if (shouldSkipCinematic) break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shouldSkipCinematic = false;

        ScenesManager.Instance.TransitionLoadTargetScene(nextScene, transitionType);
    }

    public void SkipCinematic() => shouldSkipCinematic = true;
}
