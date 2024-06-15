using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacter : Character
{
    void Start()
    {

        abilities = new List<IHeroAbility>();
        abilities.Add(new Cleave());
        abilities.Add(new HeroicBuff());
        abilities.Add(new Cleave());
        abilities.Add(new HeroicBuff());

        if (characterAttributes != null)
        {
            Debug.Log("My Name is: " + characterAttributes.characterName);
        }
    }

    void Cleave()
    {
        // Enemy is bleeding for 2 damage every combat turn
    }

    void HeroicCharge()
    {
        // Enemy Stunned % some combat turns and little Damage
    }
}
