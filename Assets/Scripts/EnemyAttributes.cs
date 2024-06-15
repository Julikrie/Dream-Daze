using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy Class")]
public class EnemyAttributes : ScriptableObject
{
    public string characterName;
    public string characterClassification;

    public Sprite characterSprite;

    public int health;
    public int mana;

    // public int physicalDamage;
    // public int magicalDamage;
    public int physicalDefense;
    public int magicalDefense;
    public int strength;
    public int intelligence;
    public int agility;
}

