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
    public string player;
    public List<string> cardList = new List<string>() { "basic_warrior.asset", "basic_warrior.asset", "old_red_mage.asset", "old_red_mage.asset", "old_red_mage.asset", "basic_warrior.asset", "basic_warrior.asset", "basic_warrior.asset" };
    // public List<CardView> MyCards = new List<CardView>();
    void Start()
    {
        ShuffleDeck();
        // drawCard(4);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void loadDeck()
    {
        foreach (string card in cardList)
        {
            Card c = AssetDatabase.LoadAssetAtPath<Card>("Assets/Prefabs/cards/" + card);
            Card newCard = c;
            myDeck.Add(newCard);
        }
    }

    void addCardToHand(Card card)
    {
        Card c = new Card(card.id, card.exp, card.level, card.cardName, card.text, card.release, card.type, card.race, card.attack,
            card.attack_mod, card.type, card.attack_range, card.max_hp, card.hp, card.effects, card.tags, card.rank, card.cost, card.art,
            card.color, card.sign, card.currentZone, this.player);

        GameObject newCard = Instantiate(cardPrefab) as GameObject;
        newCard.GetComponent<CardView>().LoadCard(c);

        newCard.transform.SetParent(handCanvas.transform);
        newCard.transform.localPosition = handCanvas.transform.localPosition;
        newCard.transform.localRotation = handCanvas.transform.localRotation;
    }

    public void drawCard(int qty)
    {
        for (int i = 0; i < qty; i++)
        {
            addCardToHand(myDeck[0]);
            myDeck.RemoveAt(0);
        }
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
