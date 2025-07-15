using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionBarUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator indicatorAnimator;

    [Header("UI Components")]
    [SerializeField] private Button backgroundButton;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float barValue;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION = "Showing";

    public Button BackgroundButton => backgroundButton;
    public float BarValue => barValue;

    public void ShowActiveIndicator()
    {
        indicatorAnimator.ResetTrigger(HIDE_TRIGGER);
        indicatorAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HideActiveIndicator()
    {
        indicatorAnimator.ResetTrigger(SHOW_TRIGGER);
        indicatorAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
