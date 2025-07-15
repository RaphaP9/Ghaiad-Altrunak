using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReselectOnClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}