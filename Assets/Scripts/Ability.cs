using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public string Name;
    public int ManaCost;
    public int Damage;
    public AnimationClip AbilityAnimation;
    private Animator animatorAttacker;
    private Animator animatorDefender;
    

     public void Use(Character current, Character other)
    {
        current.SpendMana(ManaCost);

        // Trigger the ability animation
        if (AbilityAnimation != null)
        {
            animatorAttacker = current.GetComponent<Animator>();
            animatorDefender = other.GetComponent<Animator>();
            AudioSource audioSource = other.GetComponent<AudioSource>();

            if (animatorAttacker != null)
            {
                animatorAttacker.Play(AbilityAnimation.name);
                animatorDefender.Play(other.hurtAnimation.name);
                audioSource.PlayOneShot(other.hurtSound);
                // Wait for the animation to complete before dealing damage
                current.StartCoroutine(DealDamageAfterAnimation(current, other));
                current.StartCoroutine(TakeDamageEffect(other));
            }
        }
        else
        {
            // If no animation, deal damage immediately
            DealDamage(current, other);
        }
    }

    private IEnumerator DealDamageAfterAnimation(Character current, Character other)
    {
        // Wait for the duration of the animation
        animatorAttacker.SetBool("isIdle", false);
        yield return new WaitForSeconds(AbilityAnimation.length);
        animatorAttacker.SetBool("isIdle", true);

        // Calculate and apply damage
        DealDamage(current, other);
    }

    private IEnumerator TakeDamageEffect(Character other)
    {
        animatorDefender.SetBool("isIdle", false);
        yield return new WaitForSeconds(other.hurtAnimation.length);
        animatorDefender.SetBool("isIdle", true);
    }

    private void DealDamage(Character current, Character other)
    {
        float effectiveness = RockPaperScissor.GetEffectiveness(current, other);
        other.TakeDamage(Mathf.RoundToInt(Damage * effectiveness));
    }
}