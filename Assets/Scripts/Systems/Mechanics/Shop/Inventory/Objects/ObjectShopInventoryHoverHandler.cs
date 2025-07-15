using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class ObjectShopInventoryHoverHandler : UIHoverHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private ObjectShopInventorySingleUI objectShopInventorySingleUI;
    [SerializeField] private Button button;

    [Header("Runtime Filled")]
    [SerializeField] private bool isHovered;
    [SerializeField] private bool isPressed;

    public bool IsHovered => isHovered;
    public bool IsPressed => isPressed;

    public static event EventHandler<OnObjectShopInventoryHoverEventArgs> OnObjectShopInventoryHoverEnter;
    public static event EventHandler<OnObjectShopInventoryHoverEventArgs> OnObjectShopInventoryHoverExit;

    public static event EventHandler<OnObjectShopInventoryHoverEventArgs> OnObjectShopInventoryPressed;

    public class OnObjectShopInventoryHoverEventArgs : EventArgs
    {
        public ObjectIdentified objectIdentified;
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
        OnObjectShopInventoryPressed?.Invoke(this, new OnObjectShopInventoryHoverEventArgs { objectIdentified = objectShopInventorySingleUI.ObjectIdentified });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PivotQuadrant pivotQuadrant = GetPivotQuadrantByScreenQuadrant(GeneralUtilities.GetScreenQuadrant(rectTransformRefference));

        isHovered = true;
        OnObjectShopInventoryHoverEnter?.Invoke(this, new OnObjectShopInventoryHoverEventArgs { objectIdentified = objectShopInventorySingleUI.ObjectIdentified, pivotQuadrant = pivotQuadrant });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        isPressed = false;
        OnObjectShopInventoryHoverExit?.Invoke(this, new OnObjectShopInventoryHoverEventArgs { objectIdentified = objectShopInventorySingleUI.ObjectIdentified });
    }
}
