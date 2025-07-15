using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUIContentsHandler : MonoBehaviour
{
    [Header("Contents")]
    [SerializeField] private Animator mainContentAnimator;
    [SerializeField] private Animator audioOptionsContentAnimator;
    [SerializeField] private Animator graphicsOptionsContentAnimator;
    [SerializeField] private Animator controlsOptionsContentAnimator;

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
        audioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        graphicsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        controlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowAudioOptionsContent()
    {
        audioOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        audioOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        graphicsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        controlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowGraphicsOptionsContent()
    {
        graphicsOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        graphicsOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        audioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        controlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowControlsOptionsContent()
    {
        controlsOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        controlsOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        audioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        graphicsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ResetContents()
    {
        mainContentAnimator.ResetTrigger(HIDE_TRIGGER);
        audioOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);
        graphicsOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);
        controlsOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);

        mainContentAnimator.Play(SHOWING_CONTENT_ANIMATION);
        audioOptionsContentAnimator.Play(HIDDEN_CONTENT_ANIMATION);
        graphicsOptionsContentAnimator.Play(HIDDEN_CONTENT_ANIMATION);
        controlsOptionsContentAnimator.Play(HIDDEN_CONTENT_ANIMATION);
    }
}
