using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    public Image cardBg;
    public Text cName;
    public Text cSkill;
    public Text cHealth;
    public Text cCost;
    public Text cAttack;
    public Card cCard;
    public Image borderColor;
    public bool isOnHAnd = true;
    public string player;

    //Actions buttons group
    public GameObject actionGroup;
    public GameObject fieldButtons;
    public Button ShowActionsButton;
    private GameObject gameCtrl;


    // Start is called before the first frame update
    void Start()
    {
        gameCtrl = GameObject.Find("GameController");
    }

    public void LoadCard(Card card)
    {
        if(card)
        {   
            borderColor.sprite = null;
            borderColor.color = card.color;
            cardBg.sprite = card.art;
            cardBg.color = Color.white;
            cCard = card;
            cName.text = card.cardName;
            cSkill.text = card.text;
            cHealth.text = card.hp.ToString() + " / " + card.max_hp.ToString();
            cCost.text = card.cost.ToString();
            cAttack.text = card.attack.ToString();
            player = card.player;
        }
    }

    public void updateCard()
    {
        if (cCard)
        {
            cSkill.text = cCard.text;
            cHealth.text = cCard.hp.ToString() + " / " + cCard.max_hp.ToString();
            cCost.text = cCard.cost.ToString();
            cAttack.text = cCard.attack.ToString();
            
            if (cCard.hp <= 0) {
                try
                {
                    gameCtrl.GetComponent<BattleController>().sendToGraveyard(cCard.boardPosition, player);
                    Destroy(gameObject);
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    public void EnableActionButtons()
    {
        fieldButtons.SetActive(true);
    }

    public void DisplayActions(bool alt)
    {
        if (!isOnHAnd && gameCtrl.GetComponent<BattleController>().isPlayerTurn)
        {         
           actionGroup.SetActive(alt);
        }
    }

    public void attackAction()
    {
        try
        {
           StartCoroutine(gameCtrl.GetComponent<BattleController>().showMessageGame("select enemy target"));
           gameCtrl.GetComponent<BattleController>().selectedAttacker = cCard.boardPosition;
           actionGroup.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
    public void defendAction()
    {

    }
    public void skillAction()
    {

    }
    public void inspectAction()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isOnHAnd)
        {
            Debug.Log(cCard.name + " on position: " + cCard.boardPosition);
        }
    }
}