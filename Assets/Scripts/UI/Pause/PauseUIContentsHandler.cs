using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIContentsHandler : MonoBehaviour
{
    [Header("Contents")]
    [SerializeField] private Animator mainContentAnimator;
    [SerializeField] private Animator inGameOptionsContentAnimator;
    [SerializeField] private Animator inGameAudioOptionsContentAnimator;
    [SerializeField] private Animator inGameGraphicsOptionsContentAnimator;
    [SerializeField] private Animator inGameControlsOptionsContentAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_CONTENT_ANIMATION = "Showing";
    private const string HIDDEN_CONTENT_ANIMATION = "Hidden";

    private void Start()
    {
        ResetContents();
    }

    public void ShowMainContent()
    {
        mainContentAnimator.ResetTrigger(HIDE_TRIGGER);

        mainContentAnimator.SetTrigger(SHOW_TRIGGER);
        inGameOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameAudioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameGraphicsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameControlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowInGameOptionsContent()
    {
        inGameOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        inGameOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameAudioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameGraphicsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameControlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowInGameAudioOptionsContent()
    {
        inGameAudioOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        inGameAudioOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameControlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameGraphicsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowInGameGraphicsOptionsContent()
    {
        inGameGraphicsOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        inGameGraphicsOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameAudioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameControlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowInGameControlsOptionsContent()
    {
        inGameControlsOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        inGameControlsOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameAudioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameGraphicsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ResetContents()
    {
        mainContentAnimator.ResetTrigger(HIDE_TRIGGER);
        inGameOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);
        inGameAudioOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);
        inGameGraphicsOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);
        inGameControlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);

        mainContentAnimator.Play(SHOWING_CONTENT_ANIMATION);
        inGameOptionsContentAnimator.Play(HIDDEN_CONTENT_ANIMATION);
        inGameAudioOptionsContentAnimator.Play(HIDDEN_CONTENT_ANIMATION);
        inGameGraphicsOptionsContentAnimator.Play(HIDDEN_CONTENT_ANIMATION);
        inGameControlsOptionsContentAnimator.Play(HIDDEN_CONTENT_ANIMATION);
    }
}
