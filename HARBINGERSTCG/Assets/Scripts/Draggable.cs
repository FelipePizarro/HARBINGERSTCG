using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;

public class Draggable : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // TODO: makes another draggable for enemy

    public static BattleController ctrl;
    public Transform parentToReturn = null;
    public GameObject view;
    GameObject placeholder = null;
    public string playerTurn;
    private GameObject battleCtrl;
    private bool isOnHand;
    private bool isMyCard = true;
    private CardView card;

    public void Start()
    {
        //  battleCtrl = GameObject.Find("GameController");
        //  view = GameObject.Find("CardOverviewGO");
        //  parentToReturn = GameObject.Find("hand").transform;
        card = gameObject.GetComponent<CardView>();
        ctrl = FindObjectOfType<BattleController>();
        if (!hasAuthority)
        {
            isMyCard = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("... bgin draging");
        isOnHand = gameObject.GetComponent<CardView>().isOnHAnd;
        if (isOnHand && isMyCard && ctrl.isOurTurn)
        {
            
            placeholder = new GameObject();
            placeholder.transform.SetParent(this.transform.parent);
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.flexibleHeight = 0;
            le.flexibleWidth = 0;

            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

           // view.GetComponent<cardOverview>().hideOverview();
            parentToReturn = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            if(card.cCard.type == "spell")
            {           
                List<string> spell = new List<string>(card.cCard.skill);
                Debug.Log("0 index ->" + spell[0]);
                NetworkIdentity netID = NetworkClient.connection.identity;
                My_PlayerController player = netID.GetComponent<My_PlayerController>();
                player.setSpell(spell, gameObject);      
            }
        }

        if(!isOnHand && isMyCard && ctrl.isOurTurn)
        {
            ctrl.CmdUpdateAttacker(card.cCard.boardPosition[0], card.cCard.boardPosition[1]);          
            // Debug.Log(gameObject.GetComponent<CardView>().cCard.boardPosition[0] + "/" + gameObject.GetComponent<CardView>().cCard.boardPosition[1]);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (isOnHand == true && isMyCard && ctrl.isOurTurn)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOnHand == true && isMyCard && ctrl.isOurTurn)
        {
            this.transform.SetParent(parentToReturn);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.transform.localScale = new Vector3(1, 1, 1);

            Destroy(placeholder);
        }
        if(!isOnHand && ctrl.isOurTurn && hasAuthority)
        {
            Debug.Log("end dragging card my card");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickc = eventData.clickCount;


        switch (clickc)
        {
            case 1:
                break;            
            case 2: OnDoubleClick();
                break;
        }
    }

    public void OnDoubleClick()
    {
        Card c = gameObject.GetComponent<CardView>().cCard;

        view.GetComponent<cardOverview>().showOverview(c);
    }
}