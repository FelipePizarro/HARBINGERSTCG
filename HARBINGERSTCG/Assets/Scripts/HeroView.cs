using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class HeroView : NetworkBehaviour
{
    public Hero hero;
    public TextMeshProUGUI exp_bar;
    public int exp;
    public int next_exp;
    public TextMeshProUGUI seal_name_text;
    public int hp;
    public int max_hp;
    public TextMeshProUGUI level_text;
    public TextMeshProUGUI hp_text;
    public List<string> skillList = new List<string>();
    public bool isDefeated;
    public int fortressHp;
    public TextMeshProUGUI fortress_hp_text;
    public int max_blue_mana = 0;
    public int blue_mana = 0;
    public TextMeshProUGUI blue_mana_text;
    public int max_green_mana = 0;
    public int green_mana = 0;
    public TextMeshProUGUI green_mana_text;
    public int max_yellow_mana = 0;
    public int yellow_mana = 0;
    public TextMeshProUGUI yellow_mana_text;
    public int max_red_mana = 0;
    public int red_mana = 0;
    public TextMeshProUGUI red_mana_text;
    public int max_white_mana = 0;
    public int white_mana = 0;
    public TextMeshProUGUI white_mana_text;
    public int currentConvertedMana = 0;
    public TextMeshProUGUI hero_name;
    public GameObject hero_art_img;
    public GameObject heroPanelContainer;

    public void loadHero(Card heroCard)
    {
        Debug.Log(heroCard.seal + "seal");
        hero = new Hero(heroCard.id, 20, 20, 1, 1, heroCard.seal, "", heroCard.cardName, heroCard.text, new List<string>(),
            heroCard.release, "hero", heroCard.race, heroCard.art, heroCard.color);
        hp = hero.hp;
        max_hp = hp;
        exp = 0;
        next_exp = 100;
        isDefeated = false;
        fortressHp = 30;
        setDefaultMana(heroCard.color);
        setUIelements();
    }

    public void setDefaultMana(string color)
    {
        switch (color)
        {
            case "blue":
                max_blue_mana = 1;
                blue_mana = max_blue_mana;
                break;
            case "green":
                max_green_mana = 1;
                green_mana = max_green_mana;
                break;
            case "yellow":
                max_yellow_mana = 1;
                yellow_mana = max_yellow_mana;
                break;
            case "red":
                max_red_mana = 1;
                red_mana = max_red_mana;
                break;
            default: max_white_mana = 1;
                white_mana = max_white_mana;
                break;
        }

        setConvertedManaPool();
    }

    public void setConvertedManaPool()
    {
        currentConvertedMana = blue_mana + red_mana + yellow_mana + green_mana + white_mana;
    }

    public int getConvertedManaPool()
    {
       return blue_mana + red_mana + yellow_mana + green_mana + white_mana;
    }

    public void spendMana(Card card) 
    {
        int cost = card.cost; // 4
        blue_mana -= card.costBlue; // 2
        red_mana -= card.costRed; // 2
        green_mana -= card.costGreen;
        yellow_mana -= card.costYellow;

        if(cost == 0)
        {
            updateHeroUI();
            return;
        }

        for (int i = 0; i < cost; i++)
        {
            if(white_mana > 0) { 
                white_mana -= 1;
            } else if(red_mana > 0)
            {
                red_mana -= 1;
            } else if(yellow_mana > 0)
            {
                yellow_mana -= 1;
            } else if(green_mana > 0)
            {
                green_mana -= 1;
            } else if(blue_mana > 0)
            {
                blue_mana -= 1;
            }
        }

        updateHeroUI();
    }

    public void addMana(string color, int qty)
    {
        switch (color)
        {
            case "blue":
                max_blue_mana += 1;
                blue_mana = max_blue_mana;
                break;
            case "green":
                max_green_mana += 1;
                green_mana = max_green_mana;
                break;
            case "yellow":
                max_yellow_mana += 1;
                yellow_mana = max_yellow_mana;
                break;
            case "red":
                max_red_mana += 1;
                red_mana = max_red_mana;
                break;
            case "white":
                max_white_mana += 1;
                white_mana = max_white_mana;
                break;
            default:
                break;
        }

        updateHeroUI();
    }

    public void updateHeroUI()
    {
        hp_text.text = hp + "/" + max_hp;
        level_text.text = hero.level + "";
        exp_bar.text = exp + " /" + next_exp;
        seal_name_text.text = hero.mainSeal;
        fortress_hp_text.text = fortressHp + "";
        hero_name.text = hero.heroName;
        blue_mana_text.text = blue_mana + "";
        green_mana_text.text = green_mana + "";
        red_mana_text.text = red_mana + "";
        yellow_mana_text.text = yellow_mana + "";
        white_mana_text.text = white_mana + "";
        setConvertedManaPool();
    }

    public void setUIelements()
    {
        hp_text.text = hp + "/" + max_hp;
        level_text.text = "" + hero.level;
        exp_bar.text = exp + " /" + next_exp;
        seal_name_text.text = hero.mainSeal;
        fortress_hp_text.text = fortressHp + "";
        hero_name.text = hero.heroName;
        hero_art_img.GetComponent<Image>().sprite = Resources.Load<Sprite>("cards_art/" + hero.art);

        blue_mana_text.text = blue_mana + "";
        green_mana_text.text = green_mana + "";
        red_mana_text.text = red_mana + "";
        yellow_mana_text.text = yellow_mana + "";
        white_mana_text.text = white_mana + "";
    }

    public void restoreAllMana()
    {
        red_mana = max_red_mana;
        white_mana = max_white_mana;
        yellow_mana = max_yellow_mana;
        blue_mana = max_blue_mana;
        green_mana = max_green_mana;

        updateHeroUI();
    }
}
