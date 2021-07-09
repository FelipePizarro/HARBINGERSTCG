using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class BattleController : NetworkBehaviour
{
    public static My_PlayerController playerController;
    public GameObject[,] playerField = new GameObject[3, 3];
    public GameObject[,] enemyField = new GameObject[3, 3];
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
    public List<string> cardList = new List<string>() {};
    public List<Card> playerDeck = new List<Card>();
    public List<Card> enemyDeck = new List<Card>();
    [SyncVar]
    public My_PlayerController player1;
    [SyncVar]
    public My_PlayerController player2;

    // battle ->
    public string selectedTargetPlayer;
    public bool listenForTarget = false;
    public bool isTargetSelected = false; 
    public GameObject target;


    public int playerCount = 0;
    public GameObject cardDealer;
    public int CurrentAttackerX = 4;
    public int CurrentAttackerY = 4;
    public int CurrentTargetX = 4;
    public int CurrentTargetY = 4;

    [SyncVar]
    public SyncList<string> currentSpell = new SyncList<string>();


    public int DiceRoll = 0;
    [HideInInspector] public bool isOurTurn = false;

    [SyncVar]
    public string targetType;
    [SyncVar]
    public string currentAction;

    [SyncVar]
    public bool hostStarts = false;

    [SyncVar]
    public GameObject spellcard;
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("server started");
        loadDeck();
     // drawCard(3);
    }

    [Command(ignoreAuthority = true)]
    public void CmdsetCurrentAction(string action)
    {
        currentAction = action;
    }

    [Command(ignoreAuthority = true)]
    public void CmdsetSpellGO(GameObject spell)
    {
        spellcard = spell;
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
    public void startGame(bool hostFirst)
    {    
       // My_PlayerController player = My_PlayerController.localPlayer; // to set thing on current player
        isOurTurn = true;
        turn_button.gameObject.SetActive(true);
        if(!hostFirst)
        {
            CmdEndTurn();
            //playerController.CmdDrawCard(1);
            return;
        } else
        {
            NetworkIdentity netID = NetworkClient.connection.identity;
            My_PlayerController p = netID.GetComponent<My_PlayerController>();

            p.CmdDrawCard(1);
        }
    }

    public void registerPlayers(GameObject player)
    {
        playerController = player.GetComponent<My_PlayerController>();
        if (player1 == null)
        {
            Debug.Log("registered player 1");
            player1 = player.GetComponent<My_PlayerController>();
            player1.tag = "player1";
            playerCount++;
        }
        else
        {
            Debug.Log("registered player 2");
            player2 = player.GetComponent<My_PlayerController>();
            player2.tag = "player2";
            playerCount++;
        }
    }

    void Start()
    {
        hideMessageGame();
        turn_button.gameObject.SetActive(isOurTurn);
    } 

    public void addCardToField(int[] place, GameObject card, string player)
    {
        listenForTarget = false;
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

    [Command(ignoreAuthority = true)]
    public void CmdEndTurn()
    {
        RpcSetTurn();
    }

    [ClientRpc]
    public void RpcSetTurn()
    {
        isOurTurn = !isOurTurn;
        Debug.Log("isOurTurn:  ----   " + isOurTurn);
        turn_button.gameObject.SetActive(isOurTurn);
        if(isOurTurn)
        {   
            //  playerController = My_PlayerController.localPlayer;
            // playerController.CmdDrawCard(1);
            NetworkIdentity netID = NetworkClient.connection.identity;
            My_PlayerController p = netID.GetComponent<My_PlayerController>();
            p.CmdDrawCard(1);
        }
    }

    public static My_PlayerController getIdentity()
    {
        NetworkIdentity netID = NetworkClient.connection.identity;
        return netID.GetComponent<My_PlayerController>();

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


    [Command(ignoreAuthority = true)]
    public void CmdUpdateAttacker(int x, int y)
    {
        CurrentAttackerX = x;
        CurrentAttackerY = y;

        currentAction = "attacking";
        targetType = "enemy";
    }

    [Command(ignoreAuthority = true)]
    public void CmdPrepareSpell(int x, int y, bool myCard, List<string> skill)
    {
        RpcCastSpell(x, y, myCard, skill);
        //spellcard.GetComponent<CardView>().discard();
        playerGraveyard.Add(spellcard.GetComponent<CardView>().cCard);
        Destroy(spellcard, 0.3f);
    }

    public void discard()
    {
        NetworkIdentity netID = NetworkClient.connection.identity;
        My_PlayerController p = netID.GetComponent<My_PlayerController>();
        if (p.spellcardGO)
        {
            p.spellcardGO.GetComponent<CardView>().discard();
        }
    }

    [ClientRpc]
    public void RpcCastSpell(int x, int y, bool myCard, List<string> skill)
    {
        if(isOurTurn)
        {
            foreach (var effect in skill)
            {
                string[] effects = effect.Split('+');
                string ability = effects[1];
                string qty = effects[2];
                string type = effects[3];
                string conditional = effects[4];
                string animation = effects[5];

                switch (ability)
                {
                    case "damage": 
                        if(myCard) {
                            playerField[x, y].GetComponent<CardView>().hp -= int.Parse(qty);
                            playerField[x, y].GetComponent<CardView>().updateCard();
                        }
                        else
                        {
                            enemyField[x, y].GetComponent<CardView>().hp -= int.Parse(qty);
                            enemyField[x, y].GetComponent<CardView>().updateCard();
                        }
                        break;
                    default:
                        break;
                }
            }

        } else
        {
            foreach (var effect in skill)
            {
                string[] effects = effect.Split('+');
                string ability = effects[1];
                string qty = effects[2];
                string type = effects[3];
                string conditional = effects[4];
                string animation = effects[5];

                switch (ability)
                {
                    case "damage":
                        if (!myCard)
                        {
                            playerField[x, y].GetComponent<CardView>().hp -= int.Parse(qty);
                            playerField[x, y].GetComponent<CardView>().updateCard();
                        }
                        else
                        {
                            enemyField[x, y].GetComponent<CardView>().hp -= int.Parse(qty);
                            enemyField[x, y].GetComponent<CardView>().updateCard();
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdAttackToUnit(int targetX,int targetY)
    {
        Debug.Log("****** cmd attack");
        Debug.Log(CurrentTargetX + "," + CurrentTargetY);
        RpcAttackToUnit(CurrentAttackerX, CurrentAttackerY, targetX, targetY);
    }

    [ClientRpc]
    public void RpcAttackToUnit(int CurrentAttackerX2,int CurrentAttackerY2, int CurrentTargetX2, int CurrentTargetY2)
    {
        Debug.Log(CurrentAttackerX + "," + CurrentAttackerY + " / " + CurrentTargetX + "," + CurrentTargetY);
        Debug.Log(CurrentAttackerX2 + "," + CurrentAttackerY2 + " / " + CurrentTargetX2 + "," + CurrentTargetY2);

        if (isOurTurn)
        {
            Debug.Log("attacking");
            Debug.Log(CurrentAttackerX2 + "," + CurrentAttackerY2 + " / " + CurrentTargetX2 + "," + CurrentTargetY2);

            enemyField[CurrentTargetX2, CurrentTargetY2].GetComponent<CardView>().hp -= playerField[CurrentAttackerX2, CurrentAttackerY2].GetComponent<CardView>().cCard.attack;
            playerField[CurrentAttackerX2, CurrentAttackerY2].GetComponent<CardView>().hp -= enemyField[CurrentTargetX2, CurrentTargetY2].GetComponent<CardView>().cCard.attack;

            enemyField[CurrentTargetX2, CurrentTargetY2].GetComponent<CardView>().updateCard();
            playerField[CurrentAttackerX2, CurrentAttackerY2].GetComponent<CardView>().updateCard();
        } else
        {
            Debug.Log("attacked");
            Debug.Log(CurrentAttackerX2 + "," + CurrentAttackerY2 + " / " + CurrentTargetX2 + "," + CurrentTargetY2);

            playerField[CurrentTargetX2, CurrentTargetY2].GetComponent<CardView>().hp -= enemyField[CurrentAttackerX2, CurrentAttackerY2].GetComponent<CardView>().cCard.attack;
            enemyField[CurrentAttackerX2, CurrentAttackerY2].GetComponent<CardView>().hp -= playerField[CurrentTargetX2, CurrentTargetY2].GetComponent<CardView>().cCard.attack;

            playerField[CurrentTargetX2, CurrentTargetY2].GetComponent<CardView>().updateCard();
            enemyField[CurrentAttackerX2, CurrentAttackerY2].GetComponent<CardView>().updateCard();
        }
        /*
        CurrentAttacker.GetComponent<CardView>().cCard.hp -= CurrentTarget.GetComponent<CardView>().cCard.attack;
        CurrentTarget.GetComponent<CardView>().cCard.hp -= CurrentAttacker.GetComponent<CardView>().cCard.attack;

        CurrentAttacker.GetComponent<CardView>().cCard.isDefending = false;
        CurrentAttacker.GetComponent<CardView>().attacks_number -= 1;*/

        //CurrentAttacker = null;
        //CurrentTarget = null;
    }

    public void sendToGraveyard(int[] card_pos, bool myCard)
    {
        if(myCard)
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
          //  enemyField[card_pos[0], card_pos[1]].GetComponent<Draggable>().parentToReturn.GetComponent<dropZone>().hasCard = false;
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
         //  StartCoroutine(activateSkill(skill));
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

    /*
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

    }*/

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
