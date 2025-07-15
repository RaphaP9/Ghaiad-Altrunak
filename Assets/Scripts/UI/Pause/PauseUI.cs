using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PauseUI : UILayer
{
    [Header("Components")]
    [SerializeField] private Animator pauseUIAnimator;

    [Header("UI Components")]
    [SerializeField] private Button resumeButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnCloseFromUI;
    public static event EventHandler OnPauseUIOpen;
    public static event EventHandler OnPauseUIClose;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    protected override void OnEnable()
    {
        base.OnEnable();
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
        SetUIState(State.Closed);
    }
    private void InitializeButtonsListeners()
    {
        resumeButton.onClick.AddListener(CloseFromUI);
    }

    private void InitializeVariables()
    {
        UIUtilities.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);
        AddToUILayersList();

        ShowPauseUI();
        OnPauseUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();
        HidePauseUI();

        OnPauseUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        OnCloseFromUI?.Invoke(this, EventArgs.Empty);
    }

    public void ShowPauseUI()
    {
        pauseUIAnimator.ResetTrigger(HIDE_TRIGGER);
        pauseUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HidePauseUI()
    {
        pauseUIAnimator.ResetTrigger(SHOW_TRIGGER);
        pauseUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    #region PauseManager Subscriptions
    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        OpenUI();
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        CloseUI();
    }
    #endregion
}
