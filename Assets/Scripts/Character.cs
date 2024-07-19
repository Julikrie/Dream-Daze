using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterAttributes characterAttributes;

    public int movementRange;
    public int attackRange;
    public Sprite sprite;
    public bool isDead;
    public Action<Character> tookDamage;

    public MovementStateMachine movementStateMachine { get; private set; }
    public CharacterStateMachine characterStateMachine { get; private set; }
    public AttackStateMachine attackStateMachine { get; private set; }

    private void Awake()
    {
        MovementController movementController = GetComponent<MovementController>();
        movementStateMachine = new MovementStateMachine(this, movementController);
        attackStateMachine = new AttackStateMachine(this);
        characterStateMachine = new CharacterStateMachine(movementStateMachine, attackStateMachine);
    }

    void Start()
    {
        characterAttributes.currentHealth = characterAttributes.maxHealth;
        characterAttributes.currentMana = characterAttributes.maxMana;


        var gridPosition = GridManager.Instance.GetGridFromWorldPosition(gameObject.transform.position);
        var gridCell = GridManager.Instance.GetGridCell(gridPosition);
        if (gridCell != null)
        {
            gridCell.occupant = gameObject;
            gameObject.transform.position = GridManager.Instance.GetWorldFromCellPosition(gridCell) + new Vector3(0, 0.25f, 0);
        }
    }

    void Update()
    {
        characterStateMachine.Update();
    }

    public void ToggleCharacter(bool isEnabled)
    {
        characterStateMachine.Enable(isEnabled);
        if (TeamManager.Instance.IsEnemyCharacter(this) && isEnabled)
        {
            characterStateMachine.ChangeState(CharacterState.Move);
        }
    }

    public void TakeDamage(int damage)
    {
        if (this.characterAttributes != null)
        {
            this.characterAttributes.currentHealth -= damage;
            tookDamage.Invoke(this);
            Debug.Log($"{this.characterAttributes.currentHealth} of {this.characterAttributes.maxHealth} health left. Just took {damage} damage!");
            if (this.characterAttributes.currentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            Debug.LogError($"CharacterAttributes is null in TakeDamage method!");
        }
    }

    public void SpendMana(int manaCost)
    {
        if (characterAttributes.currentMana - manaCost <= 0)
        {
            characterAttributes.currentMana -= manaCost;
        }
    }

    public void Die()
    {
        Debug.Log($"Just killed {gameObject.name}!");
        isDead = true;
    }

    public void BasicAttack(Character other)
    {
        float effectivness = RockPaperScissor.GetEffectiveness(this, other); Debug.Log("Hallo");
        Debug.Log(characterAttributes.strength / 2);
        Debug.Log(Mathf.RoundToInt(effectivness * (characterAttributes.strength / 2)));
        other.TakeDamage(Mathf.RoundToInt(effectivness *(characterAttributes.strength / 2)));
    }
 
    private void OnDestroy()
    {
        GridCell gridCell = GridManager.Instance.GetGridCell(gameObject.transform.position);
        
        if (gridCell != null)
        {
            gridCell.occupant = null;
        }

        this.characterStateMachine.ChangeState(CharacterState.Wait);
        TeamManager.Instance.RemoveCharacter(this);
        TurnManager.Instance.RemoveCharacter(this);
    }
}
