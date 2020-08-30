using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        // init player hand
        // player_hand.SetActive(true);
        turn_button.interactable = false;
        player_hand.GetComponent<PlayerController>().loadDeck();
        enemy_hand.GetComponent<PlayerController>().loadDeck();
        initGame();
    }

    public void addCardToField(int[] place, GameObject card, string player)
    {
        if(player == "player")
        {
            playerField[place[0], place[1]] = card;
            card.GetComponent<Card>().boardPosition = place;
            // check battleCry
        } else if(player == "enemy")
        {
            enemyField[place[0], place[1]] = card;
            card.GetComponent<Card>().boardPosition = place;
            // check battleCry
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
            Debug.Log("error");
            Debug.Log(e);
            throw;
        }
    }
    private void startPlayerTurn()
    {
        //  turn_button.interactable = true;
        Debug.Log("player turn");
        turn_button.GetComponentInChildren<Text>().text = "End turn";
        turn_button.interactable = true;
        isPlayerTurn = true;
        player_hand.GetComponent<PlayerController>().drawCard(1);
    }

    private void startEnemyTurn()
    {
        Debug.Log("enemy turn");
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
        } else
        {
            startPlayerTurn();
        }
    }
}
