using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    [CreateAssetMenu(menuName = "Card")]
    public class Card: ScriptableObject
    {
      public string id;
      public int exp;
      public int level;
      public string cardName;
      public string text;
      public string release;
      public string type;
      public string race;
      public int attack;
      public int attack_mod;
      public string attack_type;
      public int attack_range;
      public int max_hp;
      public int hp;
     // public  Skill[] effects;
      public string[] onSummon;
      public string[] skill;
      public string[] tags;
      public string rank;
      public int cost;
      public int costGreen;
      public int costBlue;
      public int costRed;
      public int costYellow;
      public string art;
      public string color;
      public string seal;
      public int[] boardPosition;
      public string currentZone;
      public string player;
      public bool isDefending;
      public bool isMyCard;
    public Card(string id, int exp, int level, string cardName, string text, string release, string type, string race, int attack,
        int attack_mod, string attack_type, int attack_range, int max_hp, int hp, string[] tags, string rank,
        int cost, string art, string color, string seal, string current_zone, string player, string[] onsummon, bool myCard, string[] skill,
        int blue, int green, int yellow, int red)
        {
        this.id = id;
        this.exp = exp;
        this.level = level;
        this.cardName = cardName;
        this.text = text;
        this.release = release;
        this.type = type;
        this.race = race;
        this.attack = attack;
        this.attack_mod = attack_mod;
        this.attack_range = attack_range;
        this.attack_type = attack_type;
        this.max_hp = max_hp;
        this.hp = hp;
    //    this.effects = effects;
        this.tags = tags;
        this.rank = rank;
        this.cost = cost;
        this.art = art;
        this.color = color;
        this.seal = seal;
        this.currentZone = current_zone;
        this.player = player;
        this.onSummon = onsummon;
        isDefending = true;
        this.isMyCard = myCard;
        this.skill = skill;
        this.costBlue = blue;
        this.costGreen = green;
        this.costRed = red;
        this.costYellow = yellow;
        }
    }


/*
 * id: string,
name: string,
text: string,
release: number,
type: creature | spell | item | structure,
race: string,
attack : number | null,
attack_type: string |null,
attack_range: number,
max_hp: number | null,
hp: number | null,
effect: Effect[],
tag: string[],
rank: number,
cost: number,
color: 'yellow' | 'blue' | 'red' | 'grey' | 'green',
class: '0 - 16
 */
