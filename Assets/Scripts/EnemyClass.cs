using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public string charName;
    public string className;
    public string attribute;

    public int health;
    public int strength;
    public int intelligence;
    public int agility;
    public int defense;

    void Start()
    {
        health = 30;
    }

    void Update()
    {
        
    }
}
