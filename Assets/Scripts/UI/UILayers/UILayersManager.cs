using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UILayersManager : MonoBehaviour
{
    public static UILayersManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("UI Layers")]
    [SerializeField] private List<UILayer> _UILayers;

    public List<UILayer> UILayers => _UILayers;
    private bool CloseInput => UIInput.GetPauseDown();

    public static event EventHandler<OnUILayerCloseInputEventArgs> OnUILayerCloseInput;
    public static event EventHandler OnCloseAllUILayers;

    public static event EventHandler OnUILayerActive;
    public static event EventHandler OnUILayerInactive;

    public bool UILayerActive => _UILayers.Count > 0;
    private bool previousUILayerActive;

    public class OnUILayerCloseInputEventArgs : EventArgs
    {
        public UILayer UILayerToClose;
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
        CheckUILayerToClose();
        CheckUILayerActive();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one UIManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        previousUILayerActive = UILayerActive;
    }

    public void CheckUILayerToClose()
    {
        if (!CloseInput) return;
        if (!UILayerActive) return;

        if(PauseManager.Instance != null)
        {
            if (PauseManager.Instance.GamePausedThisFrame) return;
        }
        
        OnUILayerCloseInput?.Invoke(this, new OnUILayerCloseInputEventArgs { UILayerToClose = _UILayers[^1] });
        UIInput.SetInputOnCooldown();
    }

    public void CheckUILayerActive()
    {
        bool currentUIActive = UILayerActive;

        if (currentUIActive && !previousUILayerActive)
        {
            OnUILayerActive?.Invoke(this, EventArgs.Empty);
        }

        if (!currentUIActive && previousUILayerActive)
        {
            OnUILayerInactive?.Invoke(this, EventArgs.Empty);
        }

        previousUILayerActive = currentUIActive;
    }

    public bool IsFirstOnList(UILayer UILayer)
    {
        if (_UILayers.Count == 0) return false;
        return UILayer == _UILayers[^1];
    }

    public int GetUILayersCount() => _UILayers.Count;

    public void AddToLayersList(UILayer baseUI) => _UILayers.Add(baseUI);
    public void RemoveFromLayersList(UILayer baseUI) => _UILayers.Remove(baseUI);

    private void CloseAllUIs()
    {
        OnCloseAllUILayers?.Invoke(this, EventArgs.Empty);
    }
}