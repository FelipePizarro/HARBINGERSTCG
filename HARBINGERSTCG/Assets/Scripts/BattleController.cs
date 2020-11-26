using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public GameObject[,] playerField = new GameObject[5, 3];
    public GameObject[,] enemyField = new GameObject[5, 3];
    public GameObject CardGo;
    public GameObject player_hand;
    public GameObject enemy_hand;
    public bool isPlayerTurn = false;
    public Button turn_button;
    public GameObject gameMessage;
    public List<Card> playerGraveyard = new List<Card>();
    public List<Card> enemyGraveyard = new List<Card>();

    // battle ->
    public int[] selectedAttacker = {};
    public int[] selectedDefender = {};
    
    void Start()
    {
        turn_button.interactable = false;
        player_hand.GetComponent<PlayerController>().loadDeck();
        enemy_hand.GetComponent<PlayerController>().loadDeck();
        hideMessageGame();
        
        initGame();
    }
    
    public void addCardToField(int[] place, GameObject card, string player)
    {
        try
        {
            if (player == "player")
            {
                playerField[place[0], place[1]] = card;
                card.GetComponent<CardView>().cCard.boardPosition = place;
                card.GetComponent<CardView>().EnableActionButtons();

                // check battleCry
            }
            else if (player == "enemy")
            {
                enemyField[place[0], place[1]] = card;
                card.GetComponent<CardView>().cCard.boardPosition = place;
                card.GetComponent<CardView>().EnableActionButtons();

                // check battleCry
            }
            card.GetComponent<CardView>().cCard.isDefending = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }


    private void initGame()
    {
        StartCoroutine(waiting());
        try
        {
            player_hand.GetComponent<PlayerController>().drawCard(3);
            enemy_hand.GetComponent<PlayerController>().drawCard(4);
            Debug.Log("drawing");
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
        player_hand.GetComponent<PlayerController>().drawCard(1);
    }

    private void startEnemyTurn()
    {
        StartCoroutine(showMessageGame("Enemy turn"));
        turn_button.interactable = false;
        turn_button.GetComponentInChildren<Text>().text = "Enemy turn";
        isPlayerTurn = false;
        // player_hand.SetActive(false);
        enemy_hand.GetComponent<PlayerController>().drawCard(1);
        StartCoroutine(waiting());        
    }

    private IEnumerator waiting()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Ending turn");
        finishTurn();
    }


    public void finishTurn()
    {
        foreach (GameObject item in playerField)
        {
            if(item)
            {
                item.GetComponent<CardView>().cCard.attack += 1;
                item.GetComponent<CardView>().LoadCard(item.GetComponent<CardView>().cCard);
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
        yield return new WaitForSeconds(5);
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
            playerField[card_pos[0], card_pos[1]] = null;
            Card c = AssetDatabase.LoadAssetAtPath<Card>("Assets/Prefabs/cards/" + card_name);
            Card card = c;
            playerGraveyard.Add(c);
        } else
        {
            string card_name = enemyField[card_pos[0], card_pos[1]].GetComponent<CardView>().cCard.cardName;
            enemyField[card_pos[0], card_pos[1]] = null;
            Card c = AssetDatabase.LoadAssetAtPath<Card>("Assets/Prefabs/cards/" + card_name);
            Card card = c;
            enemyGraveyard.Add(c);
        }
    
    }
}
