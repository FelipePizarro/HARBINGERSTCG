using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject handCanvas;
    public List<Card> myDeck = null;
    public List<string> cardList = new List<string>() { "basic_warrior.asset", "basic_warrior.asset", "old_red_mage.asset", "old_red_mage.asset", "old_red_mage.asset", "basic_warrior.asset", "basic_warrior.asset", "basic_warrior.asset" };
    // public List<CardView> MyCards = new List<CardView>();
    void Start()
    {
        loadDeck(cardList);
        ShuffleDeck();
        drawCard();
        drawCard();
        drawCard();
        drawCard();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene(1);
        }
    }

    void loadDeck(List<string> cardList)
    {
        foreach (string card in cardList)
        {
            Card c = AssetDatabase.LoadAssetAtPath<Card>("Assets/Prefabs/cards/" + card);
            myDeck.Add(c);
        }
    }

    void drawCard()
    {
        addCardToHand(myDeck[0]);
        myDeck.RemoveAt(0);
    }

    void addCardToHand(Card card)
    {
        Card c = card;

        GameObject newCard = Instantiate(cardPrefab) as GameObject;
        newCard.GetComponent<CardView>().LoadCard(c);

        newCard.transform.SetParent(handCanvas.transform);
        newCard.transform.localPosition = handCanvas.transform.localPosition;
        newCard.transform.localRotation = handCanvas.transform.localRotation;
    }

     void ShuffleDeck()
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
