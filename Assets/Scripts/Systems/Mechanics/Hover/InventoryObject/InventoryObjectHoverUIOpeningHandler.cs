using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObjectHoverUIOpeningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InventoryObjectHoverUIHandler inventoryObjectHoverUI;
    [SerializeField] private Animator animator;

    private const string HOVER_IN_TRIGGER = "HoverIn";
    private const string HOVER_OUT_TRIGGER = "HoverOut";

    private const string PRESS_IN_TRIGGER = "PressIn";
    private const string PRESS_OUT_TRIGGER = "PressOut";

    private void OnEnable()
    {
        inventoryObjectHoverUI.OnHoverOpening += InventoryObjectHoverUI_OnHoverOpening;
        inventoryObjectHoverUI.OnHoverClosing += InventoryObjectHoverUI_OnHoverClosing;

        inventoryObjectHoverUI.OnPressEnabling += InventoryObjectHoverUI_OnPressEnabling;
        inventoryObjectHoverUI.OnPressDisabling += InventoryObjectHoverUI_OnPressDisabling;
    }

    private void OnDisable()
    {
        inventoryObjectHoverUI.OnHoverOpening -= InventoryObjectHoverUI_OnHoverOpening;
        inventoryObjectHoverUI.OnHoverClosing -= InventoryObjectHoverUI_OnHoverClosing;

        inventoryObjectHoverUI.OnPressEnabling -= InventoryObjectHoverUI_OnPressEnabling;
        inventoryObjectHoverUI.OnPressDisabling -= InventoryObjectHoverUI_OnPressDisabling;
    }

    private void HoverIn()
    {
        animator.ResetTrigger(HOVER_OUT_TRIGGER);
        animator.ResetTrigger(PRESS_IN_TRIGGER);
        animator.ResetTrigger(PRESS_OUT_TRIGGER);
        animator.SetTrigger(HOVER_IN_TRIGGER);
    }

    private void HoverOut()
    {
        animator.ResetTrigger(HOVER_IN_TRIGGER);
        animator.ResetTrigger(PRESS_IN_TRIGGER);
        animator.ResetTrigger(PRESS_OUT_TRIGGER);
        animator.SetTrigger(HOVER_OUT_TRIGGER);
    }

    private void PressIn()
    {
        animator.ResetTrigger(HOVER_IN_TRIGGER);
        animator.ResetTrigger(HOVER_OUT_TRIGGER);
        animator.ResetTrigger(PRESS_OUT_TRIGGER);
        animator.SetTrigger(PRESS_IN_TRIGGER);
    }

    private void PressOut()
    {
        animator.ResetTrigger(HOVER_IN_TRIGGER);
        animator.ResetTrigger(HOVER_OUT_TRIGGER);
        animator.ResetTrigger(PRESS_IN_TRIGGER);
        animator.SetTrigger(PRESS_OUT_TRIGGER);
    }

    private void InventoryObjectHoverUI_OnHoverOpening(object sender, InventoryObjectHoverUIHandler.OnGenericInventoryObjectEventArgs e)
    {
        HoverIn();
    }

    private void InventoryObjectHoverUI_OnHoverClosing(object sender, InventoryObjectHoverUIHandler.OnGenericInventoryObjectEventArgs e)
    {
        HoverOut();
    }

    private void InventoryObjectHoverUI_OnPressEnabling(object sender, InventoryObjectHoverUIHandler.OnGenericInventoryObjectEventArgs e)
    {
        PressIn();
    }

    private void InventoryObjectHoverUI_OnPressDisabling(object sender, InventoryObjectHoverUIHandler.OnGenericInventoryObjectEventArgs e)
    {
        PressOut();
    }
}
