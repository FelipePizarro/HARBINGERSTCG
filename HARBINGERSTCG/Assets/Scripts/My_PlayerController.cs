using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;
using System;

public class My_PlayerController : NetworkBehaviour
{
    public GameObject ctrl;
    [HideInInspector] 
    public static BattleController ctrlScript;
    public static HeroView heroView;
    public GameObject cardPrefab;
    public GameObject handCanvas;
    public GameObject enemyCanvas;
    public GameObject turnButton;
    public GameObject Logger;
    public List<Card> myDeck = new List<Card>();
    public string player;
    public List<string> cardList = new List<string>() {};
    //public List<CardView> MyCards = new List<CardView>();
        
    public GameObject playerField;
    public GameObject enemyField;
    //[SyncVar]
    //public bool isMyTurn = false;
    [SyncVar]
    public bool readyToStart = false;

    public Card heroCard;
    public string heroName = "basic_warrior";

    // hero
    public GameObject heroContainer;

    //[SyncVar]
    public bool hostStarts = false;

    [SyncVar, HideInInspector] 
    public bool firstPlayer = false; // Is it player 1, player 2, etc.
    [HideInInspector] public static My_PlayerController localPlayer;

    public string targetType = "";
    public string currentAction = "";
    public int[] target;

    public List<string> skill = new List<string>();
    public GameObject spellcardGO;
    public Card cardToDeloy;

    void Start()
    {
        ctrlScript = FindObjectOfType<BattleController>();
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
            ctrl = GameObject.Find("BattleController");
            handCanvas = GameObject.Find("handPlayer");
            enemyCanvas = GameObject.Find("handEnemy");
            playerField = GameObject.Find("playerField");
            enemyField = GameObject.Find("enemyField");
            turnButton = GameObject.Find("turn_button");
            Logger = GameObject.Find("game_logger");
            heroContainer = GameObject.Find("playerHeroContainer");
            heroView = GameObject.Find("playerHeroView").GetComponent<HeroView>();

            ctrl.GetComponent<BattleController>().registerPlayers(gameObject);
            if (hasAuthority)
            {
                StartCoroutine(wait4AllPlayer());
                Debug.Log("initializing");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public void setSpell(List<string> currentSpell, GameObject card)
    {
        spellcardGO = card;
        string[] firstSpell = currentSpell[0].Split('+');
        targetType = firstSpell[0];
        Debug.Log(targetType);
        skill = currentSpell;
        Debug.Log(skill);
        currentAction = "spellcasting";
        ctrlScript.CmdsetCurrentAction("spellcasting");
        ctrlScript.CmdsetSpellGO(card);
        //CmdSetSpellGO(card);
    }

    public bool hasEnoughManaToPlay(Card card)
    {
        cardToDeloy = card;
        if(heroView.blue_mana < card.costBlue) { return false; }
        if(heroView.red_mana < card.costRed) { return false; }
        if(heroView.green_mana < card.costGreen) { return false; }
        if(heroView.yellow_mana < card.costYellow) { return false; }
        if(heroView.getConvertedManaPool() < card.cost) { return false; }

        return true;
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetSpellGO(GameObject card)
    {
        ctrlScript.spellcard = card;
    }

    public void castSpell(int x, int y, bool myCard)
    {
        Debug.Log("cast spell");
        Debug.Log(x + "/" + y);
        Debug.Log(skill[0]);
        Debug.Log(myCard);
        ctrlScript.CmdPrepareSpell(x, y, myCard, skill);
    }

    public void writeTextLog(String text)
    {
        Logger.GetComponent<TextMeshProUGUI>().text += Environment.NewLine + text;
    }
    


    public IEnumerator wait4AllPlayer()
    {
       while (ctrl.GetComponent<BattleController>().playerCount < 2 && !readyToStart)
       {
          yield return 0;
       }

        //TD
        GameObject g = GameObject.Find("DealerButton");
        g.GetComponent<CardDealer>().OnClick();
        initGame();
    }

    public void initGame()
    {    
        // TODO: Load deck here based on list received
        // Load Hero and  Hero UI

        if (isServer)
        {
            int rn = UnityEngine.Random.Range(1, 10);
            if (rn < 6)
            {
                // host goes first
                Debug.Log("starting game first");
                ctrlScript.hostStarts = true;
                ctrlScript.startGame(true);            
            } else
            {
                // host goes second
                ctrlScript.hostStarts = false;
                Debug.Log("starting game second");
                ctrlScript.startGame(false);
            } 
        }

        LoadHero();

        /*if(isMyTurn)
        {
            rpcStartTurn();
        } else
        {
            rpcFinish();
        }*/
    }

    public void LoadHero()
    {
        Card heroCard = Resources.Load<Card>("cards_list/" + heroName);
        heroView.loadHero(heroCard); 
    }

    public void checkFirstPlayerToGo()
    {

    }

    public void playCard(GameObject card, int[] zone)
    {
        Debug.Log(zone[0] + " " + zone[1]);
        CmdPlayCard(card, zone);
        heroView.spendMana(card.GetComponent<CardView>().cCard);
    }

    [Command]
    void CmdPlayCard(GameObject card, int[] zone)
    {
        RpcSummonCard(card, zone);
    }

    [ClientRpc]
    void RpcSummonCard(GameObject card, int[] zone)
    {
         if(hasAuthority)
        { 
            card.transform.SetParent(playerField.GetComponent<ZoneManager>().getZoneByPos(zone).transform);
            ctrl.GetComponent<BattleController>().playerField[zone[0], zone[1]] = card;
            card.GetComponent<CardView>().cCard.boardPosition = zone;
            card.GetComponent<CardView>().cardTags.Add("ally");
          //  card.GetComponent<CardView>().EnableActionButtons();
            if (card.GetComponent<CardView>().cCard.onSummon.Length > 0)
            {
               // card.GetComponent<CardView>().onSummontrigger();
            }
        } else
        {
            card.transform.SetParent(enemyField.GetComponent<ZoneManager>().getZoneByPos(zone).transform);
            ctrl.GetComponent<BattleController>().enemyField[zone[0], zone[1]] = card;
            card.GetComponent<CardView>().cCard.boardPosition = zone;
            card.GetComponent<CardView>().cardTags.Add("enemy");
            card.GetComponent<CardView>().isOnHAnd = false;

            //card.GetComponent<CardView>().EnableActionButtons();
        }

        card.GetComponent<cardFlipper>().FlipCard(false);
        checkAllcards();
         
    }

    public void checkAllcards()
    {
        foreach (var item in ctrl.GetComponent<BattleController>().playerField)
        {
            if(item)
            {
                Debug.Log(item.GetComponent<CardView>().cName.text);
            }
        }
        foreach (var item in ctrl.GetComponent<BattleController>().enemyField)
        {
            if(item)
            {
                Debug.Log(item.GetComponent<CardView>().cName.text);
            }
        }
    }

    public void loadDeck()
    {
       foreach (string card_name in cardList)
        {
            Card c = Resources.Load<Card>("cards_list/" + card_name);
            Card newCard = c;
         //   Debug.Log(newCard);
            myDeck.Add(newCard);
        }
    }

    public void askForCards()
    {
        CmdDrawCard(4);
    }


    [Command(ignoreAuthority = true)]
    public void CmdDrawCard(int qty)
    {
        if (myDeck.Count != 0)
        {
            for (int i = 0; i < qty; i++)
            {
                //  addCardToHand(myDeck[0]);
                Card card = myDeck[0];
                Card c = new Card(card.id, card.exp, card.level, card.cardName, card.text, card.release, card.type, card.race, card.attack,
                card.attack_mod, card.type, card.attack_range, card.max_hp, card.hp, card.tags, card.rank, card.cost, card.art,
                card.color, card.seal, card.currentZone, this.player, card.onSummon, true, card.skill, card.costBlue, card.costGreen, 
                card.costYellow, card.costRed);

                GameObject newCard = Instantiate(cardPrefab) as GameObject;
                //newCard.GetComponent<CardView>().LoadCard(c);
                  
                NetworkServer.Spawn(newCard, connectionToClient);
                RpcShowCards(newCard, c);
                myDeck.RemoveAt(0);
            }
        }
        else
        {
            ctrl.GetComponent<BattleController>().showMessageGame("No more cards!");
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
            card.GetComponent<CardView>().isMyCard = true;
            card.GetComponent<cardFlipper>().FlipCard(false);
        }
        else
        {
            card.transform.SetParent(enemyCanvas.transform, false);
            card.GetComponent<CardView>().isMyCard = false;
            card.transform.localPosition = handCanvas.transform.localPosition;
            card.transform.localRotation = handCanvas.transform.localRotation;
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < myDeck.Count; i++)
        {
            Card temp = myDeck[i];
            int randomIndex = UnityEngine.Random.Range(i, myDeck.Count);
            myDeck[i] = myDeck[randomIndex];
            myDeck[randomIndex] = temp;
        }
    }

    public void addMana()
    {
        heroView.addMana("white", 2);
    }

    public void restoreAllMana()
    {
        heroView.restoreAllMana();
    }
    


    /*
    [Command]
    public void cmdStartTurn()
    {
        isMyTurn = true;
        writeTextLog("my turn:" + isMyTurn + " " + gameObject.tag);
        turnButton.GetComponent<Button>().interactable = true;
        CmdDrawCard(1);
//        CmdDrawCard(1);
       // Debug.Log("starting turn");
      //  StartCoroutine(ctrl.GetComponent<BattleController>().showMessageGame("starting turn"));
    }

   // [ClientRpc]
    public void rpcFinish()
    {
        isMyTurn = false;
        writeTextLog("my turn:" + isMyTurn + " " + gameObject.tag);
        turnButton.GetComponent<Button>().interactable = false;
        StartCoroutine(ctrl.GetComponent<BattleController>().showMessageGame("ending turn"));
        ctrl.GetComponent<BattleController>().changeTurn(gameObject);
    }

    public void disableTurn()
    {
        isMyTurn = false;
        writeTextLog("my turn:" + isMyTurn + " " + gameObject.tag);
        turnButton.GetComponent<Button>().interactable = false;
    }

    public void CmdChangeTurn()
    {
        ctrl.GetComponent<BattleController>().changeTurn(gameObject);
        Debug.Log(gameObject.tag);
    }

    [ClientRpc]
    public void RpcChangeTurn()
    {
        My_PlayerController pm = NetworkClient.connection.identity.GetComponent<My_PlayerController>();
        pm.isMyTurn = !pm.isMyTurn;
        // Logger.GetComponent<Text>().text = "my turn:" + isMyTurn;
    }*/

    public bool IsOurTurn() => ctrlScript.isOurTurn;
}
