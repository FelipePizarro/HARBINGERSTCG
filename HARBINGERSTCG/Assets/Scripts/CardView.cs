using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using Mirror;
using TMPro;

public class CardView : NetworkBehaviour, IPointerClickHandler, IDropHandler
{
    public static BattleController ctrl;
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
    public bool isMyCard = false;

    public int hp = 1;
    public List<string> cardTags = new List<string>();
    public int[] boardPos;

    //mana cost
    public TextMeshProUGUI white_mana_cost;
    public TextMeshProUGUI red_mana_cost;
    public TextMeshProUGUI blue_mana_cost;
    public TextMeshProUGUI green_mana_cost;
    public TextMeshProUGUI yellow_mana_cost;

    // Start is called before the first frame update
    void Start()
    {
        //gameCtrl = GameObject.Find("BattleController");
        ctrl = FindObjectOfType<BattleController>();
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

            cardBg.sprite = Resources.Load<Sprite>("cards_art/"+card.art);
            cardBg.color = Color.white;
            cCard = card;
            cName.text = card.cardName;
            cSkill.text = card.text;
            hp = card.max_hp;
            cHealth.text = hp + " / " + card.max_hp.ToString();
            cAttack.text = card.attack.ToString();
            player = card.player;

            white_mana_cost.text = card.cost + "";
            red_mana_cost.text = card.costRed + "";
            blue_mana_cost.text = card.costBlue + "";
            green_mana_cost.text = card.costGreen + "";
            yellow_mana_cost.text = card.costYellow + "";

            if (card.cost == 0)
            {
                white_mana_cost.transform.parent.gameObject.SetActive(false);
            }
            if(card.costRed == 0)
            {
                red_mana_cost.transform.parent.gameObject.SetActive(false);
            }
            if(card.costYellow == 0)
            {
                yellow_mana_cost.transform.parent.gameObject.SetActive(false);
            }
            if(card.costGreen == 0)
            {
                green_mana_cost.transform.parent.gameObject.SetActive(false);
            }
            if(card.costBlue == 0)
            {
                blue_mana_cost.transform.parent.gameObject.SetActive(false);
            }
        }
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
            cHealth.text = hp + " / " + cCard.max_hp;
            cCost.text = cCard.cost.ToString();
            cAttack.text = cCard.attack.ToString();
            
            if (hp <= 0) {                        
                   ctrl.GetComponent<BattleController>().sendToGraveyard(cCard.boardPosition, hasAuthority);
                   Destroy(gameObject);            
            }
        }
    }

    public void discard()
    {
       // Destroy(gameObject.GetComponent<Draggable>().parentToReturn);
       if(hasAuthority)
        {
            Destroy(gameObject, 1);
        } else
        {
            Debug.LogWarning("no authority");
        }

    }

    public void EnableActionButtons()
    {
        fieldButtons.SetActive(true);
    }

    public void DisplayActions(bool alt)
    {
        /*
        Debug.Log(hasAuthority);
        if (!isOnHAnd && hasAuthority)
        {         
           actionGroup.SetActive(alt);
        }
        */
    }

    public void attackAction()
    {

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

    public void OnDrop(PointerEventData eventData)
    {
        if(isOnHAnd)
        {
            return;
        }
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        // Debug.Log(d.gameObject.GetComponent<CardView>().cName.text);
      //  Debug.Log("Drop on card" + cCard.boardPosition[0] + "/" + cCard.boardPosition[1] + " " + ctrl.currentAction);
        if (d != null &&
            ctrl.isOurTurn)
        {
            Debug.Log("switch");
            switch (ctrl.currentAction)
            {
                case "attacking":
                    if (d.GetComponent<CardView>().isOnHAnd || isMyCard)
                    {
                        return;
                    }
                    ctrl.CmdAttackToUnit(cCard.boardPosition[0], cCard.boardPosition[1]);
                    break;
                case "spellcasting":
                    if(isOnHAnd)
                    {
                        return;
                    }
                    Debug.Log("carview spellcasting");
                    Debug.Log(cCard.boardPosition[0] + " / " + cCard.boardPosition[1]);
                    NetworkIdentity netID = NetworkClient.connection.identity;
                    My_PlayerController player = netID.GetComponent<My_PlayerController>();


                    if (checkCurrentTarget(player.targetType))
                    {
                        player.castSpell(cCard.boardPosition[0], cCard.boardPosition[1], hasAuthority);
                        Debug.Log("checking tags");
                    }
                break;
                default:
                    break;
            }
        }
    }

    public bool checkCurrentTarget(string tagToCompare)
    {
        if(tagToCompare == "any")
        {
            return true;
        }
        foreach ( string tag in cardTags)
        {
            if (tag == tagToCompare)
            {
                return true;
            } 
        }
        return false;
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