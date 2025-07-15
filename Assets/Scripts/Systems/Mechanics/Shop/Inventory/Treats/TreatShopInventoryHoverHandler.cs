using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class TreatShopInventoryHoverHandler : UIHoverHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private TreatShopInventorySingleUI treatShopInventorySingleUI;
    [SerializeField] private Button button;

    [Header("Runtime Filled")]
    [SerializeField] private bool isHovered;
    [SerializeField] private bool isPressed;

    public bool IsHovered => isHovered;
    public bool IsPressed => isPressed;

    public static event EventHandler<OnTreatShopInventoryHoverEventArgs> OnTreatShopInventoryHoverEnter;
    public static event EventHandler<OnTreatShopInventoryHoverEventArgs> OnTreatShopInventoryHoverExit;

    public static event EventHandler<OnTreatShopInventoryHoverEventArgs> OnTreatShopInventoryPressed;

    public class OnTreatShopInventoryHoverEventArgs : EventArgs
    {
        public TreatIdentified treatIdentified;
        public PivotQuadrant pivotQuadrant;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        button.onClick.AddListener(OnButtonPressed);
    }

    public void OnButtonPressed()
    {
        isPressed = true;
        OnTreatShopInventoryPressed?.Invoke(this, new OnTreatShopInventoryHoverEventArgs { treatIdentified = treatShopInventorySingleUI.TreatIdentified });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PivotQuadrant pivotQuadrant = GetPivotQuadrantByScreenQuadrant(GeneralUtilities.GetScreenQuadrant(rectTransformRefference));

        isHovered = true;
        OnTreatShopInventoryHoverEnter?.Invoke(this, new OnTreatShopInventoryHoverEventArgs { treatIdentified = treatShopInventorySingleUI.TreatIdentified, pivotQuadrant = pivotQuadrant});
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        isPressed = false;
        OnTreatShopInventoryHoverExit?.Invoke(this, new OnTreatShopInventoryHoverEventArgs { treatIdentified = treatShopInventorySingleUI.TreatIdentified});
    }
}