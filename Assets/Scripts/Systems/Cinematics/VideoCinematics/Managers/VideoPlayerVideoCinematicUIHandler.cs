using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerVideoCinematicUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource videoAudioSource;

    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        VideoCinematicManager.OnCinematicPlayStart += VideoCinematicManager_OnCinematicPlayStart;
        VideoCinematicManager.OnCinematicPlayEnd += VideoCinematicManager_OnCinematicPlayEnd;
    }

    private void OnDisable()
    {
        VideoCinematicManager.OnCinematicPlayStart -= VideoCinematicManager_OnCinematicPlayStart;
        VideoCinematicManager.OnCinematicPlayEnd -= VideoCinematicManager_OnCinematicPlayEnd;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        SetVideoOutputMode();
        HideVideoUI();
    }

    private void CompletePlayVideo(VideoClip videoClip)
    {
        SetVideoClip(videoClip);
        SetVideoAudioSource(videoAudioSource);
        ShowVideoUI();
        PlayVideo();
    }

    private void CompleteStopVideo()
    {
        StopVideo();
        HideVideoUI();
        ClearVideoClip();
    }

    private void ShowVideoUI()
    {
        UIUtilities.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void HideVideoUI()
    {
        UIUtilities.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void SetVideoOutputMode() => videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

    private void SetVideoClip(VideoClip videoClip) => videoPlayer.clip = videoClip;
    private void ClearVideoClip() => videoPlayer.clip = null;
    private void SetVideoAudioSource(AudioSource audioSource) => videoPlayer.SetTargetAudioSource(0,audioSource);
    private void PlayVideo() => videoPlayer.Play();
    private void StopVideo() => videoPlayer.Stop();

    #region Subscriptions
    private void VideoCinematicManager_OnCinematicPlayStart(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        CompletePlayVideo(e.videoCinematicSO.videoClip);
    }

    private void VideoCinematicManager_OnCinematicPlayEnd(object sender, VideoCinematicManager.OnVideoCinematicEventArgs e)
    {
        CompleteStopVideo();
    }
    #endregion
}
