﻿using System.Collections;
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
      public string player;
    public Card(int id, int exp, int level, string cardName, string text, string release, string type, string race, int attack,
        int attack_mod, string attack_type, int attack_range, int max_hp, int hp, Skill[] effects, string[] tags, string rank,
        int cost, Sprite art, Color color, string sign, string current_zone, string player)
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
        this.effects = effects;
        this.tags = tags;
        this.rank = rank;
        this.cost = cost;
        this.art = art;
        this.color = color;
        this.sign = sign;
        this.currentZone = current_zone;
        this.player = player;
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
