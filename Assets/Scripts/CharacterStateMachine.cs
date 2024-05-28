using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public enum CharacterState
{
    Wait,
    Attack,
    Move,
    Idle
}
public class CharacterStateMachine
{
    public event Action Finished;

    private CharacterState currentState = CharacterState.Idle;
    private bool isEnabled;
    private MovementStateMachine movementStateMachine;
    private AttackStateMachine attackStateMachine;

    // Start is called before the first frame update
    public CharacterStateMachine(MovementStateMachine movementStateMachine, AttackStateMachine attackStateMachine)
    {
        isEnabled = false;
        this.movementStateMachine = movementStateMachine;
        this.movementStateMachine.Finished += MovementFinished;
        this.attackStateMachine = attackStateMachine;
        this.attackStateMachine.Finished += AttackFinished;

    }

    // Update is called once per frame
    public void Update()
    {
        if (isEnabled)
        {
            switch (currentState)
            {
                case CharacterState.Move:
                    TileSelector.Instance.ClearAttackMarkers();
                    movementStateMachine.Update();
                    break;
                case CharacterState.Attack:
                    TileSelector.Instance.ClearMovementMarkers();
                    Debug.Log("Ich bin CharacterStateMachine und im Attack");
                    attackStateMachine.Update();
                    break;
                case CharacterState.Wait:
                    ClearAllMarkers();
                    Finished.Invoke();
                    ChangeState(CharacterState.Idle);
                    break;
                case CharacterState.Idle:
                    ClearAllMarkers();
                    break;
            }
        }
    }
    public void ChangeState(CharacterState state)
    {
        currentState = state;
    }
    public void Enable(bool isEnabled)
    {
        this.isEnabled = isEnabled;
        movementStateMachine.Enable(isEnabled);
        attackStateMachine.Enable(isEnabled);
    }

    void MovementFinished()
    {
        ChangeState(CharacterState.Idle);
    }

    void AttackFinished()
    {
        ChangeState(CharacterState.Idle);
    }


    void ClearAllMarkers()
    {
        TileSelector.Instance.ClearAttackMarkers();
        TileSelector.Instance.ClearMovementMarkers();
    }
}