using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class BattleController : NetworkBehaviour
{
    public GameObject[,] playerField = new GameObject[5, 3];
    public GameObject[,] enemyField = new GameObject[5, 3];
    public GameObject cardPrefab;
    public GameObject CardGo;
    public GameObject player_hand;
    public GameObject enemy_hand;
   // public GameObject player_Deck;
   // public GameObject enemy_Deck;
    public bool isPlayerTurn = false;
    public Button turn_button;
    public GameObject gameMessage;
    public List<Card> playerGraveyard = new List<Card>();
    public List<Card> enemyGraveyard = new List<Card>();
    public List<string> cardList = new List<string>() { "basic_warrior", "basic_warrior", "old_red_mage", "old_red_mage", "old_red_mage", "basic_warrior", "basic_warrior", "basic_warrior" };
    public List<Card> playerDeck = new List<Card>();
    public List<Card> enemyDeck = new List<Card>();

    // battle ->
    public int[] selectedAttacker = {};
    public int[] selectedDefender = {};
    public int[] selectedTarget = {};
    public string selectedTargetPlayer;
    public bool listenForTarget = false;
    public bool isTargetSelected = false;
    public GameObject target;
    public string targetType = "";
    public GameObject player1 = null;
    public GameObject player2 = null;


    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("server started");
        loadDeck();

     //   drawCard(3);

    }

    public void checkPlayers(GameObject player)
    {
        if(player1 == null)
        {
            player1 = player;
            Debug.Log("added player 1");
        } else
        {
            player2 = player;
            Debug.Log("added player 2");
            RpcInitGame();
        }
    }

    public void loadDeck()
    {
        foreach (string card_name in cardList)
        {
            try
            {
 
                Card c = Resources.Load<Card>(card_name);
                Card newCard = c;
                Debug.Log(newCard);
                playerDeck.Add(newCard);

            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }

    void addCardToHand(Card card, bool isplayer)
    {
        Card c = new Card(card.id, card.exp, card.level, card.cardName, card.text, card.release, card.type, card.race, card.attack,
            card.attack_mod, card.type, card.attack_range, card.max_hp, card.hp, card.tags, card.rank, card.cost, card.art,
            card.color, card.sign, card.currentZone, "", card.onSummon);

        GameObject newCard = Instantiate(cardPrefab) as GameObject;
        newCard.GetComponent<CardView>().
            LoadCard(c);

        if(isplayer)
        {
            newCard.transform.SetParent(player_hand.transform);
        }
        else
        {
            newCard.transform.SetParent(enemy_hand.transform);
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmddrawCard(int qty, bool player)
    {
        if(playerDeck.Count > 0)
        {
            for (int i = 0; i < qty; i++)
            {
                addCardToHand(playerDeck[0], player);
                playerDeck.RemoveAt(0);
            }

        }
        else
        {
            Debug.Log("no more cards");
        }
    }

    void Start()
    {
      
        turn_button.interactable = false;
      //  player_hand.GetComponent<PlayerController>().loadDeck();
      //  enemy_hand.GetComponent<PlayerController>().loadDeck();
        hideMessageGame();
      //  target = new GameObject();
        initGame();
    } 

    public void addCardToField(int[] place, GameObject card, string player)
    {
        listenForTarget = false;
        selectedTarget = null;
        selectedTargetPlayer = "";

        try
        {
            if (player == "player")
            {
                playerField[place[0], place[1]] = card;
                card.GetComponent<CardView>().cCard.boardPosition = place;
                card.GetComponent<CardView>().EnableActionButtons();
                if (card.GetComponent<CardView>().cCard.onSummon.Length > 0)
                {
                    card.GetComponent<CardView>().onSummontrigger();
                }
            }
            else if (player == "enemy")
            {
                enemyField[place[0], place[1]] = card;
                card.GetComponent<CardView>().cCard.boardPosition = place;
                card.GetComponent<CardView>().EnableActionButtons();
                if (card.GetComponent<CardView>().cCard.onSummon.Length > 0)
                {
                    card.GetComponent<CardView>().onSummontrigger();
                }
            }
            card.GetComponent<CardView>().cCard.isDefending = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    [ClientRpc]
    public void RpcInitGame()
    {
        player1.GetComponent<My_PlayerController>().askForCards();
        player2.GetComponent<My_PlayerController>().askForCards();
    }

    private void initGame()
    {

        /*
        StartCoroutine(waiting());
        try
        {
            player_hand.GetComponent<PlayerController>().loadDeck();
            player_hand.GetComponent<PlayerController>().ShuffleDeck();
            player_hand.GetComponent<PlayerController>().drawCard(3);
            enemy_hand.GetComponent<PlayerController>().loadDeck();
            enemy_hand.GetComponent<PlayerController>().ShuffleDeck();
            enemy_hand.GetComponent<PlayerController>().drawCard(3);
          //  enemy_hand.GetComponent<PlayerController>().drawCard(4);
            Debug.Log("drawing");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }*/
    }

    public void setPlayersOder()
    {
        // TODO
    }


    private void startPlayerTurn()
    {
        //  turn_button.interactable = true;
        StartCoroutine(showMessageGame("Player turn"));
        turn_button.GetComponentInChildren<Text>().text = "End turn";
        turn_button.interactable = true;
        isPlayerTurn = true;
        player_hand.GetComponent<My_PlayerController>().CmdDrawCard(1);
        setAttacks("player");
    }

    private void startEnemyTurn()
    {
        StartCoroutine(showMessageGame("Enemy turn"));
        turn_button.interactable = false;
        turn_button.GetComponentInChildren<Text>().text = "Enemy turn";
        isPlayerTurn = false;
        // player_hand.SetActive(false);
        enemy_hand.GetComponent<My_PlayerController>().CmdDrawCard(1);
        setAttacks("enemy");
        StartCoroutine(waiting());
    }

    private IEnumerator waiting()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Ending turn");
        finishTurn();
    }

    public void setAttacks(string player) {

        if(player == "player")
        {
            foreach (GameObject item in playerField)
            {
                if(item != null)
                {
                    item.GetComponent<CardView>().restoreAttacks();
                }
            }
        }
    }

    public void finishTurn()
    {
        foreach (GameObject item in playerField)
        {
            if(item)
            {
                //item.GetComponent<CardView>().cCard.attack += 1;
                //item.GetComponent<CardView>().LoadCard(item.GetComponent<CardView>().cCard);
            }
        }

        if (isPlayerTurn == true)
        {
            startEnemyTurn();
        }
        else
        {
            startPlayerTurn();
        }
    }

    public IEnumerator showMessageGame(string msg)
    {
        gameMessage.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        gameMessage.SetActive(true);
        yield return new WaitForSeconds(7);
        hideMessageGame();
    }

    public void hideMessageGame()
    {
        gameMessage.SetActive(false);
    }

    public void attackToUnit()
    {
        playerField[selectedAttacker[0], selectedAttacker[1]].GetComponent<CardView>().cCard.hp -= enemyField[selectedDefender[0], selectedDefender[1]].GetComponent<CardView>().cCard.attack;
        enemyField[selectedDefender[0], selectedDefender[1]].GetComponent<CardView>().cCard.hp -= playerField[selectedAttacker[0], selectedAttacker[1]].GetComponent<CardView>().cCard.attack;

        playerField[selectedAttacker[0], selectedAttacker[1]].GetComponent<CardView>().cCard.isDefending = false;
        playerField[selectedAttacker[0], selectedAttacker[1]].GetComponent<CardView>().attacks_number -= 1;
        playerField[selectedAttacker[0], selectedAttacker[1]].GetComponent<CardView>().updateCard();
        enemyField[selectedDefender[0], selectedDefender[1]].GetComponent<CardView>().updateCard();

        selectedDefender = new int[0];
        selectedAttacker = new int[0];
    }

    public void sendToGraveyard(int[] card_pos, string player)
    {
        if(player == "player")
        {
            string card_name = playerField[card_pos[0], card_pos[1]].GetComponent<CardView>().cCard.cardName;
            playerField[card_pos[0], card_pos[1]].GetComponent<Draggable>().parentToReturn.GetComponent<dropZone>().hasCard = false;
            playerField[card_pos[0], card_pos[1]] = null;
            Card c = Resources.Load<Card>("Resources/" + card_name);
            Card card = c;
            playerGraveyard.Add(c);
        } else
        {
            string card_name = enemyField[card_pos[0], card_pos[1]].GetComponent<CardView>().cCard.cardName;
            enemyField[card_pos[0], card_pos[1]].GetComponent<Draggable>().parentToReturn.GetComponent<dropZone>().hasCard = false;
            enemyField[card_pos[0], card_pos[1]] = null;
            Card c = Resources.Load<Card>("Resources/" + card_name);
            Card card = c;
            enemyGraveyard.Add(c);
        }   
    }

    //effecs
 

    public void hideDisplayButtons()
    {
        foreach (GameObject item in playerField)
        {
            if(item != null)
            {
                item.GetComponent<CardView>().actionGroup.SetActive(false);
            }
        }
        
        foreach (GameObject item in enemyField)
        {
            if(item != null)
            {
                item.GetComponent<CardView>().actionGroup.SetActive(false);
            }
        }
    }

    public void pickRandom(string target)
    {

    }

    public void skillManager(string[] skill_list, string skill_trigger = "null")
    {
        foreach (string skill in skill_list)
        {
           StartCoroutine(activateSkill(skill));
        }
    }


    /*
        0.- target
        1.- effect
        2.- qty
        3.- type
        4.- conditional
        5.- animation
     */
    public IEnumerator activateSkill(string compressed_skill)
    {
        string[] skill = compressed_skill.Split('+');
        bool needTarget = false;

        //Pick Target
        switch (skill[0])
        {
            // <
            case "any": 
                needTarget = true;
                listenForTarget = true;
                targetType = skill[0];

                break;
            case "friend_unit":
                needTarget = true;
                listenForTarget = true;
                targetType = skill[0];

                break;
            case "enemy_unit":
                needTarget = true;
                listenForTarget = true;
                targetType = skill[0];

                break;
            case "enemy":
                needTarget = true;
                listenForTarget = true;
                targetType = skill[0];

                break;
            case "friend":
                needTarget = true;
                listenForTarget = true;
                targetType = skill[0];

                break;
                // > need select target
            case "random_enemy":
                break;
            case "random_unit":
                break;
            case "random_friend":
                break;
            case "hero_friend":
                break;
            case "hero_enemy":
                break;
            case "enemy_board":
                break;
            case "friend_board":
                break;
            case "all_friend":
                break;
            case "all_enemy":
                break;
            case "all_unit":
                break;
            case "all":
                break;

            default:
                break;
        }

        if (needTarget)
        {
            showMessageGame("select target" + skill[0]);
            while (!isTargetSelected)
            {
                yield return 0;
            }
        }

        listenForTarget = false;

        if (target != null)
        {
            switch (skill[1])
            {

                case "damage":
                    target.GetComponent<CardView>().receiveDamage(int.Parse(skill[2]), skill[3]);
                    break;
                default:
                    break;
            }

        }
        else Debug.Log("null target");

        target = null;
        isTargetSelected = false;

    }

    public void cancelTargetEffect()
    {
        target = null;
        isTargetSelected = true;

    }

    //web script example 
    /*
    [Server]
    public override void OnStartServer()
    {
       // cards.Add(Ping);
    }
    */

    
}
