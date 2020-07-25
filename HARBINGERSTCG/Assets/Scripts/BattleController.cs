using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    // Start is called before the first frame update


    public Card[,] playerField = new Card[5, 3];
    public Card[,] enemyField = new Card[5, 3];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addCardToField(int[] place, Card card, string player)
    {
        if(player == "player")
        {
            playerField[place[0], place[1]] = card;
            // check battleCry
        } else if(player == "enemy")
        {
            enemyField[place[0], place[1]] = card;
            // check battleCry
        }
    }
}
