using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class RehighlightOnPointerUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    private Selectable selectable;
    private bool isHovered;

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // If the pointer is still over the button, re-apply the highlighted state
        if (isHovered && selectable.interactable)
        {
            // Trick Unity into redrawing the highlight
            selectable.OnPointerExit(eventData);
            selectable.OnPointerEnter(eventData);
        }
    }
}