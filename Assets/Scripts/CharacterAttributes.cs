using System.Collections;
using System.Collections.Generic;
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

    public List<Ability> abilities = new List<Ability>();
    public Ability basicAttack;

}
