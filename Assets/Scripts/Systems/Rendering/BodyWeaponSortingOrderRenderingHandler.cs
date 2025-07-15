using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyWeaponSortingOrderRenderingHandler : SortingOrderRenderingHandler
{
    [Header("Components")]
    [SerializeField] private EntityFacingDirectionHandler entityFacingDirectionHandler;
    [Space]
    [SerializeField] private List<SpriteRenderer> bodySpriteRenderers;
    [SerializeField] private List<SpriteRenderer> weaponSpriteRenderers;
    [SerializeField] private bool respectListingOrder;
    [Space]
    [SerializeField, Range(-1f, 1f)] private float yMinFacingDirectionToBeLookingUp;

    private bool lookingUp = false;

    protected override void UpdateSortingOrder(int sortingOrder)
    {
        if ((sortingOrder == previousSortingOrder) && (IsLookingUp() == lookingUp)) return;

        int aditionalOrder = 0;

        foreach (SpriteRenderer spriteRenderer in bodySpriteRenderers)
        {
            spriteRenderer.sortingOrder = respectListingOrder ? sortingOrder + aditionalOrder : sortingOrder;
            aditionalOrder++;
        }

        int lowestOrder = sortingOrder;
        int highestOrder = sortingOrder + aditionalOrder;

        if (IsLookingUp())
        {
            RenderWeaponsBehind(lowestOrder);
            lookingUp = true;
        }
        else
        {
            RenderWeaponsOnTop(highestOrder);
            lookingUp = false;
        }
    }

    private void RenderWeaponsOnTop(int startingSortingOrder)
    {
        startingSortingOrder++;
        int aditionalOrder = 0;

        foreach (SpriteRenderer spriteRenderer in weaponSpriteRenderers)
        {
            spriteRenderer.sortingOrder = respectListingOrder ? startingSortingOrder + aditionalOrder : startingSortingOrder;
            aditionalOrder++;
        }
    }

    private void RenderWeaponsBehind(int startingSortingOrder)
    {
        startingSortingOrder--;
        int aditionalOrder = 0;

        foreach (SpriteRenderer spriteRenderer in weaponSpriteRenderers)
        {
            spriteRenderer.sortingOrder = respectListingOrder ? startingSortingOrder + aditionalOrder : startingSortingOrder;
            aditionalOrder--;
        }
    }

    private bool IsLookingUp() => entityFacingDirectionHandler.CurrentRawFacingDirection.y >= yMinFacingDirectionToBeLookingUp;
}
