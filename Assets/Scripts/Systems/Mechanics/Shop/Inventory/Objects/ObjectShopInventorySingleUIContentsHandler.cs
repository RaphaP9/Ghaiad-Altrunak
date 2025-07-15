using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectShopInventorySingleUIContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ObjectShopInventorySingleUI objectShopInventorySingleUI;
    [Space]
    [SerializeField] private Image borderImage;
    [SerializeField] private Image objectImage;

    [Header("Settings")]
    [SerializeField] private Sprite commonBorder;
    [SerializeField] private Sprite uncommonBorder;
    [SerializeField] private Sprite rareBorder;
    [SerializeField] private Sprite epicBorder;
    [SerializeField] private Sprite legendaryBorder;


    private void OnEnable()
    {
        objectShopInventorySingleUI.OnObjectInventorySet += ObjectShopInventorySingleUI_OnObjectInventorySet;
    }

    private void OnDisable()
    {
        objectShopInventorySingleUI.OnObjectInventorySet -= ObjectShopInventorySingleUI_OnObjectInventorySet;
    }

    private void SetContents(ObjectIdentified objectIdentified)
    {
        SetObjectImage(objectIdentified.objectSO.sprite);
        SetBorderSpriteByRarity(objectIdentified.objectSO);
    }

    private void SetObjectImage(Sprite sprite) => objectImage.sprite = sprite;

    private void SetBorderSpriteByRarity(ObjectSO objectSO)
    {
        Sprite sprite;

        switch (objectSO.objectRarity)
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

    private void ObjectShopInventorySingleUI_OnObjectInventorySet(object sender, ObjectShopInventorySingleUI.OnObjectInventorySetEventArgs e)
    {
        SetContents(e.objectIdentified);
    }
}
