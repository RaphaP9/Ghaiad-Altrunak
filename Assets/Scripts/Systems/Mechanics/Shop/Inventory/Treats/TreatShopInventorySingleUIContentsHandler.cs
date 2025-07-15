using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreatShopInventorySingleUIContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TreatShopInventorySingleUI treatShopInventorySingleUI;
    [Space]
    [SerializeField] private Image borderImage;
    [SerializeField] private Image treatImage;

    [Header("Settings")]
    [SerializeField] private Sprite commonBorder;
    [SerializeField] private Sprite uncommonBorder;
    [SerializeField] private Sprite rareBorder;
    [SerializeField] private Sprite epicBorder;
    [SerializeField] private Sprite legendaryBorder;


    private void OnEnable()
    {
        treatShopInventorySingleUI.OnTreatInventorySet += TreatShopInventorySingleUI_OnTreatInventorySet;
    }

    private void OnDisable()
    {
        treatShopInventorySingleUI.OnTreatInventorySet -= TreatShopInventorySingleUI_OnTreatInventorySet;
    }

    private void SetContents(TreatIdentified treatIdentified)
    {
        SetTreatImage(treatIdentified.treatSO.sprite);
        SetBorderSpriteByRarity(treatIdentified.treatSO);
    }

    private void SetTreatImage(Sprite sprite) => treatImage.sprite = sprite;

    private void SetBorderSpriteByRarity(TreatSO treatSO)
    {
        Sprite sprite;

        switch (treatSO.objectRarity)
        {
            case Rarity.Common:
            default:
                sprite = commonBorder;
                break;
            case Rarity.Uncommon:
                sprite = uncommonBorder;
                break;
            case Rarity.Rare:
                sprite = rareBorder;
                break;
            case Rarity.Epic:
                sprite = epicBorder;
                break;
            case Rarity.Legendary:
                sprite = legendaryBorder;
                break;
        }

        borderImage.sprite = sprite;
    }

    private void TreatShopInventorySingleUI_OnTreatInventorySet(object sender, TreatShopInventorySingleUI.OnTreatInventorySetEventArgs e)
    {
        SetContents(e.treatIdentified);
    }
}
