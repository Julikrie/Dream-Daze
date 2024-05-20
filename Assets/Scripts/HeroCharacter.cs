using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacter : MonoBehaviour
{
    public int health;
    public int mana;

    private string heroName = "Filler";
    private string jobClass = "Job";


    void Start()
    {
        Debug.Log("His name is " + heroName);
    }

    void Update()
    {
        
    }
}
