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
   
    void Start()
    {
        battleCtrl = GameObject.Find("BattleController");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!card.GetComponent<CardView>().isOnHAnd &&
            battleCtrl.GetComponent<BattleController>().listenForTarget)
        {
            glowEffect.GetComponent<Image>().color = Color.green;
        }
        else if(!card.GetComponent<CardView>().isOnHAnd &&
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

        if (battleCtrl.GetComponent<BattleController>().listenForTarget)
        {
            battleCtrl.GetComponent<BattleController>().target = getTargetOnClick();
            if(battleCtrl.GetComponent<BattleController>().target !=  null)
            {
                battleCtrl.GetComponent<BattleController>().isTargetSelected = true;
            }
        }
        else if(battleCtrl.GetComponent<BattleController>().selectedAttacker.Length > 0 &&
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

    private GameObject getTargetOnClick()
    {
        if (battleCtrl.GetComponent<BattleController>().listenForTarget && !card.GetComponent<CardView>().isOnHAnd)
        {
            switch (battleCtrl.GetComponent<BattleController>().targetType)
            {
                case "any":
                    return gameObject;
                case "friend_unit":
                    if (card.GetComponent<CardView>().player == "player" && gameObject.GetComponent<CardView>().isHero == false)
                    {
                        return card;
                    }
                    else return null;
                case "enemy_unit":
                    if (card.GetComponent<CardView>().player == "enemy" && gameObject.GetComponent<CardView>().isHero == false)
                    {
                        return card;
                    }
                    else return null;
                case "enemy":
                    if (card.GetComponent<CardView>().player == "enemy")
                    {
                        return card;
                    }
                    else return null;
                case "friend":
                    if (card.GetComponent<CardView>().player == "player")
                    {
                        return card;
                    }
                    else return null;
                default:
                    break;
            }

        }
        Debug.Log(gameObject.name);

        return null;
    }



    public void OnPointerExit(PointerEventData eventData)
    {
       //  card.GetComponent<CardView>().DisplayActions(false);        
        glowEffect.SetActive(false);
    }
}
