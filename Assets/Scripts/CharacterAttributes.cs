using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(fileName ="New Character", menuName = "Character Class")]
public class CharacterAttributes : ScriptableObject
{
    public string characterName;
    public string characterClass;
    public CharacterType characterType;

    //  public Sprite characterSprite;

    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;

    // public int physicalDamage;
    // public int magicalDamage;
    public int physicalDefense;
    public int magicalDefense;
    public int strength;
    public int intelligence;
    public int agility;

    public List<Ability> abilities = new List<Ability>();

}
