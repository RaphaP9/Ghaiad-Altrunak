using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickyHoverButton : Button
{
    private bool isPointerInside = false;
    private bool shouldDeselect = false;

    private void LateUpdate()
    {
        HandleDeselection();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isPointerInside = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        isPointerInside = false;

        DoStateTransition(SelectionState.Normal, false);
        ForceDeselection();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (!interactable || EventSystem.current == null) return;

        if (isPointerInside)
        {
            DoStateTransition(SelectionState.Highlighted, false);
        }
        else
        {
            DoStateTransition(SelectionState.Normal, false);
            ForceDeselection();
        }
    }

    private void ForceDeselection()
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject) return;
        
        shouldDeselect = true;
        
    }

    private void HandleDeselection()
    {
        if (shouldDeselect && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            shouldDeselect = false;
        }
    }
}