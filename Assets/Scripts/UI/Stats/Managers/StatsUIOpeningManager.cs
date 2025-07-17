using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsUIOpeningManager : MonoBehaviour
{
    public static StatsUIOpeningManager Instance {  get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("Settings")]
    [SerializeField] private bool startOpen;
    [SerializeField] private bool setInputOnCooldown;

    [Header("Runtime Filled")]
    [SerializeField] private bool isOpen;

    private bool StatsInput => UIInput.GetStatsDown();
    public bool HasOpenedThisFrame {  get; private set; }
    public bool IsOpen => isOpen;

    public static event EventHandler OnStatsUIOpen;
    public static event EventHandler OnStatsUIClose;

    public static event EventHandler OnStatsUIOpenInmediately;
    public static event EventHandler OnStatsUICloseInmediately;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
        HandleStartOpen();
    }

    private void Update()
    {
        HandleOpenCloseStats();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one StatsUIOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        SetIsOpen(false);
        HasOpenedThisFrame = false;
    }

    private void HandleStartOpen()
    {
        if (startOpen)
        {
            OpenStatsInmediately();
        }
        else
        {
            CloseStatsInmediately();
        }
    }

    private void HandleOpenCloseStats()
    {
        HasOpenedThisFrame = false;

        if (!StatsInput) return;
        if(!CanListenToInput()) return;

        if (!isOpen)
        {
            OpenStats();
            HasOpenedThisFrame = true;

            if(setInputOnCooldown) UIInput.SetInputOnCooldown();
        }
        else
        {
            CloseStats();

            if (setInputOnCooldown) UIInput.SetInputOnCooldown();
        }
    }

    private bool CanListenToInput()
    {
        return true;
    }

    private void OpenStatsInmediately()
    {
        OnStatsUIOpenInmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }
    private void CloseStatsInmediately()
    {
        OnStatsUICloseInmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }

    private void OpenStats()
    {
        OnStatsUIOpen?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }

    private void CloseStats()
    {
        OnStatsUIClose?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }

    private bool SetIsOpen(bool isOpen) => this.isOpen = isOpen;
}
