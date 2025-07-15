using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class PriceTextHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Settings")]
    [SerializeField] private Color affordableColor;
    [SerializeField] private Color cantPurchaseColor;

    [Header("Runtime Filled")]
    [SerializeField] private int currentPriceTag;

    protected virtual void OnEnable()
    {
        GoldManager.OnGoldAdded += GoldManager_OnGoldAdded;
        GoldManager.OnGoldSpent += GoldManager_OnGoldSpent;
    }
    protected virtual void OnDisable()
    {
        GoldManager.OnGoldAdded -= GoldManager_OnGoldAdded;
        GoldManager.OnGoldSpent -= GoldManager_OnGoldSpent;
    }

    protected void UpdatePriceTag(int price)
    {
        currentPriceTag = price;
        priceText.text = price.ToString();
    }

    protected void UpdatePriceColor()
    {
        if (GoldManager.Instance.CanSpendGold(currentPriceTag))
        {
            priceText.color = affordableColor;
        }
        else
        {
            priceText.color = cantPurchaseColor;
        }
    }

    #region Subscriptions
    private void GoldManager_OnGoldAdded(object sender, GoldManager.OnGoldChangedEventArgs e)
    {
        UpdatePriceColor();
    }

    private void GoldManager_OnGoldSpent(object sender, GoldManager.OnGoldChangedEventArgs e)
    {
        UpdatePriceColor();
    }
    #endregion
}
