using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using Mirror;

public class CardView : NetworkBehaviour, IPointerClickHandler
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
    public bool isHero = false;

    //Actions buttons group
    public GameObject actionGroup;
    public GameObject fieldButtons;
    public Button ShowActionsButton;
    private GameObject gameCtrl;

    //Card Status
    public int attacks_number = 0;
    public bool canAttack = true;
    public int attack_number_by_default = 1;
    public bool hasOnSummon;


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
            switch (card.color)
            {
                case "red": borderColor.color = Color.red;
                    break;                
                case "blue": borderColor.color = Color.blue;
                    break;
                default: borderColor.color = Color.black;
                    break;
            }

            cardBg.sprite = Resources.Load<Sprite>("cards/"+card.art);
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

    public void helloworld()
    {
        Debug.Log("hello!!");
    }

    public void restoreAttacks()
    {
        if (canAttack)
        {
            attacks_number = attack_number_by_default;
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
        if (!isOnHAnd && gameCtrl.GetComponent<BattleController>().isPlayerTurn && player == "player")
        {         
           actionGroup.SetActive(alt);
        }
    }

    public void attackAction()
    {
        gameCtrl.GetComponent<BattleController>().hideDisplayButtons();
        try
        {
            if (attacks_number > 0)
            {
                StartCoroutine(gameCtrl.GetComponent<BattleController>().showMessageGame("select enemy target"));
                gameCtrl.GetComponent<BattleController>().selectedAttacker = cCard.boardPosition;
            }
            else 
            {
                StartCoroutine(gameCtrl.GetComponent<BattleController>().showMessageGame("no attacks lefts"));
            }
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

    public void receiveDamage(int qty, string type = "null")
    {
        cCard.hp -= qty;
        updateCard();
    }

    public void onSummontrigger()
    {   
        Debug.Log("on summoning");
        if (cCard.onSummon[0] != null)
        {
            // effect
            gameCtrl.GetComponent<BattleController>().skillManager(cCard.onSummon);
        }
    }
}