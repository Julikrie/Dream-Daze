using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class MageClass : MonoBehaviour
{
    public string charName;
    public string className;
    public string attribute;
    public int health;
    public int strength;
    public int intelligence;
    public int agility;
    public int defense;

    public void Awake()
    {
        charName = "Flipper";
        className = "Mage";
        attribute = "Paper";
        health = 27;
        strength = 7;
        intelligence = 14;
        agility = 9;
        defense = 8;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fireball();
        }
    }

    void Fireball(int baseDamage = 7)
    {
        int fireBallDamage = baseDamage + (intelligence / 2);
        Debug.Log(fireBallDamage + "Fireballschaden");
    }
}