using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeroAbility
{
    string Name { get; }
    int manaCost { get; }
    public void Use(Character current, Character other);
}

public class Cleave : IHeroAbility
{
    public string Name { get; } = "Cleave";
    int damage = 7;
    public int manaCost { get; } = 2; // test

    public void Use(Character current, Character other)
    {
        current.SpendMana(manaCost);
        other.TakeDamage(damage);
    }
}

public class HeroicBuff : IHeroAbility
{
    public string Name { get; } = "Heroic Buff";
    int damage = 7;
    public int manaCost { get; } = 2;

    public void Use(Character current, Character other)
    {

        current.characterAttributes.strength *= 2;
        // how many turns
    }
}

public class PokeEye : IHeroAbility
{
    public string Name { get; } = "Poke Eye";
    int damage = 10;
    public int manaCost { get; } = 2;

    public void Use(Character current, Character other)
    {
        current.SpendMana(manaCost);
        other.TakeDamage(damage);
    }
}

public class HighKick : IHeroAbility
{
    public string Name { get; } = "High Kick";
    int damage = 7;
    public int manaCost { get; } = 4;

    public void Use(Character current, Character other)
    {
        current.SpendMana(manaCost);
        other.TakeDamage(damage);
    }
}