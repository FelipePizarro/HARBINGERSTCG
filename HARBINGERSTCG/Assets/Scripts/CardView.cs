using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CardView : MonoBehaviour
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
    public string player = "player";


    // Start is called before the first frame update
    void Start()
    {
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
        }
    }
}