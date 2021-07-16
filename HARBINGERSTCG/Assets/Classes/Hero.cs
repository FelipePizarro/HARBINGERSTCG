using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hero")]
public class Hero : ScriptableObject
{
    public string id;
    public int hp;
    public int max_hp;
    public int level;
    public int rank;
    public string mainSeal;
    public string secondSeal;
    public string heroName;
    public string heroDescription;
    public List<string> skills;
    public string release; // expansion/set number order
    public string type;
    public string race;
    public string art;
    public string color;

    public Hero(string id, int hp, int max_hp, int level, int rank, string mainSeal, string secondSeal, 
        string heroName, string heroDescription, List<string> skills, string release, string type, 
        string race, string art, string color)
    {
        this.id = id;
        this.hp = hp;
        this.max_hp = max_hp;
        this.level = level;
        this.rank = rank;
        this.mainSeal = mainSeal;
        this.secondSeal = secondSeal;
        this.heroName = heroName;
        this.heroDescription = heroDescription;
        this.skills = skills;
        this.release = release;
        this.type = type;
        this.race = race;
        this.art = art;
        this.color = color;
    }
}
