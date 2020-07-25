using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    [CreateAssetMenu(menuName = "Card")]
    public class Card: ScriptableObject
    {
      public int id;
      public int exp;
      public int level;
      public string cardName;
      public string text;
      public string release;
      public string type;
      public  string race;
      public  int attack;
      public  int attack_mod;
      public  string attack_type;
      public  int attack_range;
      public  int max_hp;
      public  int hp;
      public  Skill[] effects;
      public  string[] tags;
      public  string rank;
      public int cost;
      public Sprite art;
      public Color color;
      public string sign;
      public int[] boardPosition;
      public string currentZone;
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
