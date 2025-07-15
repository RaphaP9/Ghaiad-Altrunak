using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMenuOpeningManager : MonoBehaviour
{
    public static DevMenuOpeningManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("Settings")]
    [SerializeField] private bool canOpenDevMenu;

    public static event EventHandler OnDevMenuOpen;
    public static event EventHandler OnDevMenuClose;

    private bool DevMenuInput => UIInput.GetDevMenuDown();

    public bool CanOpenDevMenu => canOpenDevMenu;
    public bool DevMenuOpen { get; private set; }

    private void OnEnable()
    {
        DevMenuUI.OnCloseFromUI += DevMenuUI_OnCloseFromUI;
    }

    private void OnDisable()
    {
        DevMenuUI.OnCloseFromUI -= DevMenuUI_OnCloseFromUI;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckOpenCloseDevMenu();
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
        DevMenuOpen = false;
    }

    private void CheckOpenCloseDevMenu()
    {
        if (!canOpenDevMenu) return;
        if (!DevMenuInput) return;

        if (!DevMenuOpen)
        {
            if (UILayersManager.Instance.UILayerActive) return; //UILayersManager should not have any layer active

            OpenDevMenu();
            UIInput.SetInputOnCooldown();
        }
        else
        {
            if (UILayersManager.Instance.GetUILayersCount() != 1) return; //If count is 1, the active layer is the DevMenuUI, this script should not have a refference to the DevMenuUI

            CloseDevMenu();
            UIInput.SetInputOnCooldown();
        }
    }

    private void OpenDevMenu()
    {
        OnDevMenuOpen?.Invoke(this, EventArgs.Empty);
        DevMenuOpen = true;
    }

    private void CloseDevMenu()
    {
        OnDevMenuClose?.Invoke(this, EventArgs.Empty);
        DevMenuOpen = false;
    }


    #region DevMenuUI Subscriptions

    private void DevMenuUI_OnCloseFromUI(object sender, EventArgs e)
    {
        CloseDevMenu();
    }
    #endregion
}
