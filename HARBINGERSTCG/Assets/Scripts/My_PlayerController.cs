using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using Mirror;

public class My_PlayerController : NetworkBehaviour
{

    public GameObject ctrl;
   // public BattleController ctrlScript;
    public GameObject cardPrefab;
    public GameObject handCanvas;
    public GameObject enemyCanvas;
    public List<Card> myDeck = new List<Card>();
    public string player;
    public List<string> cardList = new List<string>() { "basic_warrior", "basic_warrior", "old_red_mage", "old_red_mage", "old_red_mage", "basic_warrior", "basic_warrior", "basic_warrior" };
    // public List<CardView> MyCards = new List<CardView>();
    void Start()
    {

        /*loadDeck();
         ShuffleDeck();
         // drawCard(4);*/
        handCanvas = GameObject.Find("handPlayer");
        enemyCanvas = GameObject.Find("handEnemy");
        ctrl = GameObject.Find("BattleController");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("player server");
    }

    public override void OnStartClient()
    {
        try
        {
            base.OnStartClient();
            loadDeck();
            ShuffleDeck();

            Debug.Log("player ctrller client started");
        //    ctrl = GameObject.Find("BattleController");
        //    ctrl.GetComponent<BattleController>().checkPlayers(gameObject);

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }

    }

    public void loadDeck()
    {
       foreach (string card_name in cardList)
        {
            Card c = Resources.Load<Card>(card_name);
            Card newCard = c;
            Debug.Log(newCard);
            myDeck.Add(newCard);
        }
    }

    public void askForCards()
    {
//        ctrl.GetComponent<BattleController>().CmddrawCard(2, isLocalPlayer);
    }

    void addCardToHand(Card card)
    {

    }

    [Command]
    public void CmdDrawCard(int qty)
    {
        for (int i = 0; i < qty; i++)
        {
            //  addCardToHand(myDeck[0]);
            Card card = myDeck[0];
            Card c = new Card(card.id, card.exp, card.level, card.cardName, card.text, card.release, card.type, card.race, card.attack,
            card.attack_mod, card.type, card.attack_range, card.max_hp, card.hp, card.tags, card.rank, card.cost, card.art,
            card.color, card.sign, card.currentZone, this.player, card.onSummon);

            GameObject newCard = Instantiate(cardPrefab) as GameObject;
            //newCard.GetComponent<CardView>().LoadCard(c);

            NetworkServer.Spawn(newCard, connectionToClient);
            RpcShowCards(newCard, c);
            myDeck.RemoveAt(0);
        }
    }

    [ClientRpc]
    public void RpcShowCards(GameObject card, Card c)
    {
        card.GetComponent<CardView>().LoadCard(c);
        if (hasAuthority)
        {
            card.transform.SetParent(handCanvas.transform, false);
            card.transform.localPosition = handCanvas.transform.localPosition;
            card.transform.localRotation = handCanvas.transform.localRotation;
        }
        else
        {
            card.transform.SetParent(enemyCanvas.transform, false);
            card.transform.localPosition = handCanvas.transform.localPosition;
            card.transform.localRotation = handCanvas.transform.localRotation;

        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < myDeck.Count; i++)
        {
            Card temp = myDeck[i];
            int randomIndex = Random.Range(i, myDeck.Count);
            myDeck[i] = myDeck[randomIndex];
            myDeck[randomIndex] = temp;
        }
    }
}
