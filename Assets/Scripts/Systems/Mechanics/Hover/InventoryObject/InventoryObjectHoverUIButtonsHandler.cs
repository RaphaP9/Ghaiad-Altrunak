using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryObjectHoverUIButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InventoryObjectHoverUIHandler inventoryObjectHoverUIHandler;
    [Space]
    [SerializeField] private Button sellButton;
    [SerializeField] private Button backButton;

    public event EventHandler OnBackButtonPressed;
    public event EventHandler OnSellButtonPressed;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        sellButton.onClick.AddListener(HandleSell);
        backButton.onClick.AddListener(HandleBack);
    }

    private void HandleBack()
    {
        OnBackButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void HandleSell()
    {
        if (inventoryObjectHoverUIHandler.HasRegisteredGenericInventoryObject())
        {
            switch (inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified.inventoryObjectSO.GetInventoryObjectType())
            {
                case InventoryObjectType.Object:
                    ShopSeller.Instance.SellObject(new ObjectIdentified { assignedGUID = inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified.assignedGUID, objectSO = inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified.inventoryObjectSO as ObjectSO });
                    break;
                case InventoryObjectType.Treat:
                    ShopSeller.Instance.SellTreat(new TreatIdentified { assignedGUID = inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified.assignedGUID, treatSO = inventoryObjectHoverUIHandler.CurrentGenericInventoryObjectIdentified.inventoryObjectSO as TreatSO });
                    break;
            }
        }

        OnSellButtonPressed?.Invoke(this, EventArgs.Empty);
    }
}
