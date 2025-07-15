using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObjectPurchaseVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShopObjectCardPurchaseHandler shopObjectCardPurchaseHandler;

    [Header("UI")]
    [SerializeField] private CanvasGroup cardUICanvasGroup;
    [SerializeField] private CanvasGroup purchaseUICanvasGroup;

    private void OnEnable()
    {
        shopObjectCardPurchaseHandler.OnShopObjectPurchase += ShopObjectCardPurchaseHandler_OnShopObjectPurchase;
    }

    private void OnDisable()
    {
        shopObjectCardPurchaseHandler.OnShopObjectPurchase -= ShopObjectCardPurchaseHandler_OnShopObjectPurchase;
    }

    private void HandlePurchaseUI()
    {   
        /*
        UIUtilities.SetCanvasGroupAlpha(cardUICanvasGroup, 0f);
        cardUICanvasGroup.interactable = false;
        cardUICanvasGroup.blocksRaycasts = false;
        */

        UIUtilities.SetCanvasGroupAlpha(purchaseUICanvasGroup, 1f);
        purchaseUICanvasGroup.interactable = true;
        purchaseUICanvasGroup.blocksRaycasts = true;
    }


    private void ShopObjectCardPurchaseHandler_OnShopObjectPurchase(object sender, ShopObjectCardPurchaseHandler.OnShopObjectPurchaseEventArgs e)
    {
        HandlePurchaseUI();
    }
}
