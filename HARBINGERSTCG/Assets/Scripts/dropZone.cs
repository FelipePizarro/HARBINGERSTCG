using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string zone;
    public bool hasCard;
    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
    public void OnDrop(PointerEventData eventData)
    {
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        Debug.Log(d.name + " dropped");
        if (d != null && d.GetComponent<CardView>().isOnHAnd && d.GetComponent<CardView>().player == zone && !hasCard)
            {
            d.parentToReturn = this.transform;
            d.GetComponent<CardView>().isOnHAnd = false;
            hasCard = true;
        }
    }
}
