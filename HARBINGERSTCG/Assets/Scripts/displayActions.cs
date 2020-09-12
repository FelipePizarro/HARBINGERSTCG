using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class displayActions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject card;
    public GameObject glowEffect;
    public GameObject battleCtrl;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!card.GetComponent<CardView>().isOnHAnd &&
            battleCtrl.GetComponent<BattleController>().selectedAttacker.Length > 0 && 
            card.GetComponent<CardView>().cCard.player == "enemy")
        {
            glowEffect.GetComponent<Image>().color = Color.red;
        } else
        {
            glowEffect.GetComponent<Image>().color = Color.white;
        }
        glowEffect.SetActive(true);
    }

    public void DisplayActions()
    {
        if(battleCtrl.GetComponent<BattleController>().selectedAttacker.Length > 0 &&
            card.GetComponent<CardView>().cCard.player == "enemy")
        {
            Debug.Log("attacking this unit");
            battleCtrl.GetComponent<BattleController>().selectedDefender = card.GetComponent<CardView>().cCard.boardPosition;
            battleCtrl.GetComponent<BattleController>().attackToUnit();
        }
        else
        {
            card.GetComponent<CardView>().DisplayActions(true);
        }

        glowEffect.SetActive(false);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       //  card.GetComponent<CardView>().DisplayActions(false);        
        glowEffect.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        battleCtrl = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
