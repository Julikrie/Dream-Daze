using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public int ManaCost;
    [SerializeField] public int Damage;

    private void OnEnable()
    {
        Debug.Log("Skill correctly set up" + Name + ", Mana Cost: " + ManaCost);
    }
        public void Use(Character current, Character other)
    {
        current.SpendMana(ManaCost);
        float effectiveness = RockPaperScissor.GetEffectiveness(current, other);
        other.TakeDamage(Mathf.RoundToInt(Damage * effectiveness));
    }
}