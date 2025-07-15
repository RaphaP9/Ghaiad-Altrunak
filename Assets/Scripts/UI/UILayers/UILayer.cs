using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UILayer : MonoBehaviour
{
    [Header("UILayer Settings")]
    [SerializeField] private bool canCloseFromUI;

    protected enum State { Closed, Opening, Open, Closing }

    [SerializeField] protected State state;

    protected virtual void OnEnable()
    {
        UILayersManager.OnUILayerCloseInput += UILayersManager_OnUIToCloseInput;
        UILayersManager.OnCloseAllUILayers += UILayersManager_OnCloseAllUIs;
    }

    protected virtual void OnDisable()
    {
        UILayersManager.OnUILayerCloseInput -= UILayersManager_OnUIToCloseInput;
        UILayersManager.OnCloseAllUILayers -= UILayersManager_OnCloseAllUIs;
    }

    protected void AddToUILayersList() => UILayersManager.Instance.AddToLayersList(this);
    protected void RemoveFromUILayersList() => UILayersManager.Instance.RemoveFromLayersList(this);

    protected void SetUIState(State state) => this.state = state;

    protected abstract void CloseFromUI();

    #region UIManager Subscriptions
    private void UILayersManager_OnUIToCloseInput(object sender, UILayersManager.OnUILayerCloseInputEventArgs e)
    {
        if (e.UILayerToClose != this) return;
        if (state != State.Open) return;
        if(!canCloseFromUI) return;

        CloseFromUI();
    }

    private void UILayersManager_OnCloseAllUIs(object sender, System.EventArgs e)
    {
        CloseFromUI();
    }

    #endregion
}
