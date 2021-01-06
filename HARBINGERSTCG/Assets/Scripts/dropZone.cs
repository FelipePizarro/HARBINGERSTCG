using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class dropZone : NetworkBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string zone;
    public bool hasCard;
    public string zoneId;
    public int zoneCol;
    public int zoneRow;
    public GameObject battleController;

    void Start()
    {
       battleController = GameObject.Find("BattleController");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
    public void OnDrop(PointerEventData eventData)
    {
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        // Debug.Log(d.gameObject.GetComponent<CardView>().cName.text);

        if (d != null && d.GetComponent<CardView>().isOnHAnd && 
            hasAuthority &&
            !hasCard
            )
        {
            d.parentToReturn = this.transform;
            d.GetComponent<CardView>().isOnHAnd = false;
            hasCard = true;
            battleController.GetComponent<BattleController>().addCardToField(
                new int[] { zoneCol, zoneRow },
                d.gameObject, 
                d.GetComponent<CardView>().player);
        }
    }
}
