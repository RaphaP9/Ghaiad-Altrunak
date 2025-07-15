using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsOpeningManager : MonoBehaviour
{
    public static OptionsOpeningManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;
    [SerializeField] private Button optionsButton;

    public static event EventHandler OnOptionsOpen;
    public static event EventHandler OnOptionsClose;

    public bool OptionsOpen { get; private set; }

    private void OnEnable()
    {
        OptionsUI.OnCloseFromUI += OptionsUI_OnCloseFromUI;
    }

    private void OnDisable()
    {
        OptionsUI.OnCloseFromUI -= OptionsUI_OnCloseFromUI;
    }

    private void Awake()
    {
        SetSingleton();
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one DevMenuOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        OptionsOpen = false;
    }

    private void InitializeButtonsListeners()
    {
        optionsButton.onClick.AddListener(OpenOptions);
    }

    private void OpenOptions()
    {
        if (OptionsOpen) return;
        if (UILayersManager.Instance.UILayerActive) return; //UILayersManager should not have any layer active

        Open();
        UIInput.SetInputOnCooldown();
    }

    private void Open()
    {
        OnOptionsOpen?.Invoke(this, EventArgs.Empty);
        OptionsOpen = true;
    }

    private void Close()
    {
        OnOptionsClose?.Invoke(this, EventArgs.Empty);
        OptionsOpen = false;
    }


    #region DevMenuUI Subscriptions

    private void OptionsUI_OnCloseFromUI(object sender, EventArgs e)
    {
        Close();
    }
    #endregion
}
