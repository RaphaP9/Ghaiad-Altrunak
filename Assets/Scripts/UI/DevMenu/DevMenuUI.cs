using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevMenuUI : UILayer
{
    [Header("Components")]
    [SerializeField] private Animator devMenuUIAnimator;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnCloseFromUI;
    public static event EventHandler OnDevMenuUIOpen;
    public static event EventHandler OnDevMenuUIClose;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    protected override void OnEnable()
    {
        base.OnEnable();
        DevMenuOpeningManager.OnDevMenuOpen += DevMenuOpeningManager_OnDevMenuOpen;
        DevMenuOpeningManager.OnDevMenuClose += DevMenuOpeningManager_OnDevMenuClose;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DevMenuOpeningManager.OnDevMenuOpen -= DevMenuOpeningManager_OnDevMenuOpen;
        DevMenuOpeningManager.OnDevMenuClose -= DevMenuOpeningManager_OnDevMenuClose;
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
        closeButton.onClick.AddListener(CloseFromUI);
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
        OnDevMenuUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();
        HidePauseUI();

        OnDevMenuUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        OnCloseFromUI?.Invoke(this, EventArgs.Empty);
    }

    public void ShowPauseUI()
    {
        devMenuUIAnimator.ResetTrigger(HIDE_TRIGGER);
        devMenuUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HidePauseUI()
    {
        devMenuUIAnimator.ResetTrigger(SHOW_TRIGGER);
        devMenuUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    #region DevMenuOpeningManager Subscriptions
    private void DevMenuOpeningManager_OnDevMenuOpen(object sender, System.EventArgs e)
    {
        OpenUI();
    }

    private void DevMenuOpeningManager_OnDevMenuClose(object sender, System.EventArgs e)
    {
        CloseUI();
    }
    #endregion
}
