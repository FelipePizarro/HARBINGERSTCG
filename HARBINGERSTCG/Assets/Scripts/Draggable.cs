using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform parentToReturn = null;
    public GameObject view;
    GameObject placeholder = null;

    public void Start()
    {
      //  view = GameObject.Find("CardOverviewGO");
      //  parentToReturn = GameObject.Find("hand").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("... bgin draging");
        bool onhand = gameObject.GetComponent<CardView>().isOnHAnd;
        if (onhand == true)
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
        bool onhand = gameObject.GetComponent<CardView>().isOnHAnd;
        if (onhand == true)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("...end draging");
        try
        {
            bool onhand = gameObject.GetComponent<CardView>().isOnHAnd;
            this.transform.SetParent(parentToReturn);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.transform.localScale = new Vector3(1, 1, 1);

            Destroy(placeholder);

        }
        catch (System.Exception)
        {

            Debug.Log("not draggable");
        }
      
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickc = eventData.clickCount;

     //   Debug.Log(gameObject.name);

        switch (clickc)
        {
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