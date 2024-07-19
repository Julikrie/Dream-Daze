using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string Name { get; }
    public int manaCost { get; }
    public abstract void Use(Character current, Character other);
}

[CreateAssetMenu(fileName = "Fireball", menuName = "Abilities/Fireball")]
public class Fireball : Ability
{
    public new string Name { get; } = "Fireball";
    int damage = 7;
    public new int manaCost { get; } = 2; // test

    public override void Use(Character current, Character other)
    {
        current.SpendMana(manaCost);
        float effectiveness = RockPaperScissor.GetEffectiveness(current, other);
        other.TakeDamage(Mathf.RoundToInt(damage * effectiveness));
    }
}

[CreateAssetMenu(fileName = "IceNova", menuName = "Abilities/IceNova")]
public class IceNova : Ability
{
    public new string Name { get; } = "Ice Nova";
    int damage = 7;
    public new int manaCost { get; } = 2;

    public override void Use(Character current, Character other)
    {
        current.characterAttributes.strength *= 2;
    }
}

[CreateAssetMenu(fileName = "PlatzhalterSkill", menuName = "Abilities/PlatzhalerSkill")]
public class PlatzhalterSkill : Ability
{
    public new string Name { get; } = "Platzhalter SKill";
    int damage = 10;
    public new int manaCost { get; } = 2;

    public override void Use(Character current, Character other)
    {
        current.SpendMana(manaCost);
        other.TakeDamage(damage);
    }
}

[CreateAssetMenu(fileName = "EnergeticShot", menuName = "Abilities/EnergeticShot")]

public class EnergeticShot : Ability
{
    public new string Name { get; } = "Energetic Shot";
    int damage = 7;
    public new int manaCost { get; } = 2; // test

    public override void Use(Character current, Character other)
    {
        current.SpendMana(manaCost);
        float effectiveness = RockPaperScissor.GetEffectiveness(current, other);
        other.TakeDamage(Mathf.RoundToInt(damage * effectiveness));
    }
}
[CreateAssetMenu(fileName = "LazySmash", menuName = "Abilities/LazySmash")]

public class LazySmash : Ability
{
    public new string Name { get; } = "Lazy Smash";
    int damage = 7;
    public new int manaCost { get; } = 2; // test

    public override void Use(Character current, Character other)
    {
        current.SpendMana(manaCost);
        float effectiveness = RockPaperScissor.GetEffectiveness(current, other);
        other.TakeDamage(Mathf.RoundToInt(damage * effectiveness));
    }
}
