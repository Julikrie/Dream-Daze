using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int movementRange;
    public int attackRange;
    public Sprite sprite;
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

    void Update()
    {
        characterStateMachine.Update();
    }

    public void ToggleCharacter(bool isEnabled)
    {
        characterStateMachine.Enable(isEnabled);
    }
}
