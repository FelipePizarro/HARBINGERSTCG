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

    public Transform parentToReturn = null;
    public GameObject view;
    GameObject placeholder = null;
    public string playerTurn;
    private GameObject battleCtrl;
    private bool isOnHand;
    private bool isDraggable = true;

    public void Start()
    {
       //  battleCtrl = GameObject.Find("GameController");
      //  view = GameObject.Find("CardOverviewGO");
      //  parentToReturn = GameObject.Find("hand").transform;
      if(!hasAuthority)
        {
            isDraggable = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("... bgin draging");
        isOnHand = gameObject.GetComponent<CardView>().isOnHAnd;
        if (isOnHand == true && isOnHand == true && isDraggable)
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
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("...draging");
        if (isOnHand == true && isOnHand == true && isDraggable)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (isOnHand == true && isOnHand == true && isDraggable)
        {
            this.transform.SetParent(parentToReturn);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.transform.localScale = new Vector3(1, 1, 1);

            Destroy(placeholder);

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