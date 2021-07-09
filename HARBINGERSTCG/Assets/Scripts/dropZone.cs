using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class dropZone : NetworkBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public My_PlayerController playerController;
    public string zone;
    public bool hasCard;
    public string zoneId;
    public int zoneCol;
    public int zoneRow;
    public static BattleController ctrl;

    void Start()
    {
        ctrl = FindObjectOfType<BattleController>();
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

        if (d != null && 
            d.GetComponent<CardView>().isOnHAnd && 
            !hasCard && 
            ctrl.isOurTurn &&
            d.GetComponent<CardView>().isMyCard &&
            d.GetComponent<CardView>().cCard.type != "spell"
            )
        {
            if (zone == "enemy")
            {
                return;
            }
            d.parentToReturn = this.transform;
            d.GetComponent<CardView>().isOnHAnd = false;
            hasCard = true;
            /*battleController.GetComponent<BattleController>().addCardToField(
                new int[] { zoneCol, zoneRow },
                d.gameObject, 
                d.GetComponent<CardView>().player);
            */
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerController = networkIdentity.GetComponent<My_PlayerController>();
            playerController.playCard(d.gameObject, new int[] { zoneCol, zoneRow });

        }
        if (d != null &&
            d.GetComponent<CardView>().isOnHAnd &&
            !hasCard &&
            ctrl.isOurTurn &&
            d.GetComponent<CardView>().isMyCard &&
            d.GetComponent<CardView>().cCard.type == "spell")
        {
            // ex: summon thing on empty space
        }
    }
}
