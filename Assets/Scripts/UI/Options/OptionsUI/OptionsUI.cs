using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OptionsUI : UILayer
{
    [Header("Components")]
    [SerializeField] private Animator optionsUIAnimator;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnCloseFromUI;
    public static event EventHandler OnOptionsUIOpen;
    public static event EventHandler OnOptionsUIClose;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    protected override void OnEnable()
    {
        base.OnEnable();
        OptionsOpeningManager.OnOptionsOpen += OptionsOpeningManager_OnOptionsOpen;
        OptionsOpeningManager.OnOptionsClose += OptionsOpeningManager_OnOptionsClose;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OptionsOpeningManager.OnOptionsOpen -= OptionsOpeningManager_OnOptionsOpen;
        OptionsOpeningManager.OnOptionsClose -= OptionsOpeningManager_OnOptionsClose;
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

        ShopOptionsUI();
        OnOptionsUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();
        HideOptionsUI();

        OnOptionsUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        OnCloseFromUI?.Invoke(this, EventArgs.Empty);
    }

    public void ShopOptionsUI()
    {
        optionsUIAnimator.ResetTrigger(HIDE_TRIGGER);
        optionsUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HideOptionsUI()
    {
        optionsUIAnimator.ResetTrigger(SHOW_TRIGGER);
        optionsUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    #region OptionsOpeningManager Subscriptions
    private void OptionsOpeningManager_OnOptionsOpen(object sender, System.EventArgs e)
    {
        OpenUI();
    }

    private void OptionsOpeningManager_OnOptionsClose(object sender, System.EventArgs e)
    {
        CloseUI();
    }
    #endregion
}
