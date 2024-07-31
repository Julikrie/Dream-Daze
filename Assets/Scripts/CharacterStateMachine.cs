using System;


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

    public CharacterStateMachine(MovementStateMachine movementStateMachine, AttackStateMachine attackStateMachine)
    {
        isEnabled = false;
        this.movementStateMachine = movementStateMachine;
        this.movementStateMachine.FinishedPlayer += MovementFinishedPlayer;
        this.movementStateMachine.FinishedAI += MovementFinishedAI;
        this.attackStateMachine = attackStateMachine;
        this.attackStateMachine.FinishedPlayer += AttackFinishedPlayer;
        this.attackStateMachine.FinishedAI += AttackFinishedAI;

    }

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
        movementStateMachine.Enable(isEnabled);
        attackStateMachine.Enable(isEnabled);
        currentState = state;
    }
    public void Enable(bool isEnabled)
    {
        this.isEnabled = isEnabled;
        movementStateMachine.Enable(isEnabled);
        attackStateMachine.Enable(isEnabled);
    }

    void MovementFinishedPlayer()
    {
        ChangeState(CharacterState.Idle);
    }
    void MovementFinishedAI()
    {
        ChangeState(CharacterState.Attack);
    }

    void AttackFinishedPlayer()
    {
        ChangeState(CharacterState.Idle);
    }
    void AttackFinishedAI()
    {
        ChangeState(CharacterState.Wait);
    }


    void ClearAllMarkers()
    {
        TileSelector.Instance.ClearAttackMarkers();
        TileSelector.Instance.ClearMovementMarkers();
    }
}
