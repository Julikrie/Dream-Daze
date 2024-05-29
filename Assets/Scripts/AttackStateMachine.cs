using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AttackState
{
    Selection,
    Attack,
    Finished,
    Idle
}

public class AttackStateMachine
{
    public event Action Finished;

    private AttackState currentState;
    private bool isEnabled;
    private Character character;
    private List<GridCell> attackableCells;


    public AttackStateMachine(Character character)
    {
        isEnabled = false;
        this.character = character;
        this.attackableCells = new List<GridCell>();
    }
    public void ChangeState(AttackState state)
    {
        currentState = state;
    }

    // When in AttackState show tile selector for attack range and clear when finished
    public void Update()
    {
        if (isEnabled)
        {
            switch (currentState)
            {
                case AttackState.Selection:
                    attackableCells = TileSelector.Instance.HighlightAttackRange(character.transform.position, character.attackRange);
                    ChangeState(AttackState.Attack);
                    break;

                case AttackState.Attack:
                    if (Input.GetMouseButtonDown(0))
                    {
                        GridCell gridCell = GridManager.Instance.GetGridCellFromMousePosition();
                        if (gridCell != null && attackableCells.Contains(gridCell))
                        {
                            TileSelector.Instance.ClearAttackMarkers();
                            Character enemy = gridCell.GetCharacter();
                            if (enemy != null)
                            {
                                BattleManager.Instance.InitiateBattle(character.gameObject, gridCell.occupant);
                            }
                            ChangeState(AttackState.Finished);
                        }
                    }
                    break;

                case AttackState.Finished:
                    Finished.Invoke();
                    ChangeState(AttackState.Selection);
                    break;

                case AttackState.Idle:
                    break;
            }
        }
    }
    public void Enable(bool isEnabled)
    {
        ChangeState(AttackState.Selection);
        this.isEnabled = isEnabled;
    }
}
