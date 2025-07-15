using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObjectPriceTextHandler : PriceTextHandler
{
    [Header("Components")]
    [SerializeField] private ShopObjectCardUI shopObjectCardUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        shopObjectCardUI.OnInventoryObjectSet += ShopObjectCardUI_OnInventoryObjectSet;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        shopObjectCardUI.OnInventoryObjectSet -= ShopObjectCardUI_OnInventoryObjectSet;
    }


    private void ShopObjectCardUI_OnInventoryObjectSet(object sender, ShopObjectCardUI.OnInventoryObjectEventArgs e)
    {
        UpdatePriceTag(e.inventoryObjectSO.price);
        UpdatePriceColor();
    }
}
