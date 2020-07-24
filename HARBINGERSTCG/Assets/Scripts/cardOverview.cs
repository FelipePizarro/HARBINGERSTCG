using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cardOverview : MonoBehaviour, IPointerClickHandler
{

    private bool hidden = false;
    // Start is called before the first frame update
    void Start()
    {
       hideOverview();
    }

   public void showOverview(Card obj)
    {
        gameObject.GetComponent<CardView>().LoadCard(obj);

        if (hidden)
        {
            gameObject.transform.position = new Vector3((gameObject.transform.position.x + 500f), gameObject.transform.position.y);
            hidden = false;
        }
    }

    public void hideOverview()
    {
        if(!hidden)
        {
            gameObject.transform.position = new Vector3((gameObject.transform.position.x - 500f), gameObject.transform.position.y);
            hidden = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hideOverview();
    }
}
