using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AttackState
{
    Selection,
    Attack,
    Finished,
}

public class AttackStateMachine
{

    public event Action FinishedPlayer;
    public event Action FinishedAI;

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
            if (TeamManager.Instance.IsPlayerCharacter(character))
            {
                HandlePlayerAttack();
            }
            else
            {

                AICoroutineManager.Instance.RunCoroutine(HandleAIAttack());
            }
        }
    }
    public void Enable(bool isEnabled)
    {
        ChangeState(AttackState.Selection);
        this.isEnabled = isEnabled;
    }

    private void HandlePlayerAttack()
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
                    if (gridCell != null && attackableCells.Contains(gridCell) &&
                        gridCell.occupant != null && !TeamManager.Instance.SameTeam(character, gridCell.GetCharacter()))
                    {
                        TileSelector.Instance.ClearAttackMarkers();
                        Character enemy = gridCell.GetCharacter();
                        if (enemy != null)
                        {
                            BattleManager.Instance.InitiateBattle(character.gameObject, gridCell.occupant,() => ActionUIManager.Instance.SetCanvasActive(true));
                        }
                        ActionUIManager.Instance.DisableAttackButton();
                        ChangeState(AttackState.Finished);
                    }
                }
                ActionUIManager.Instance.ToggleCanvasOnClick();
                break;

            case AttackState.Finished:
                FinishedPlayer.Invoke();
                ChangeState(AttackState.Selection);
                break;
        }
    }

    private IEnumerator HandleAIAttack()
    {
        switch (currentState)
        {
            case AttackState.Selection:
                attackableCells = TileSelector.Instance.HighlightAttackRange(character.transform.position, character.attackRange);
                yield return new WaitForSeconds(0.5f);
                ChangeState(AttackState.Attack);
                break;

            case AttackState.Attack:
                if (!BattleManager.Instance.IsBattleOngoing)
                {
                    TileSelector.Instance.ClearAttackMarkers();
                    foreach (var cell in attackableCells)
                    {
                        if (cell != null && cell.occupant != null && !TeamManager.Instance.SameTeam(character, cell.GetCharacter()))
                        {
                            Character enemy = cell.GetCharacter();
                            if (enemy != null)
                            {
                                BattleManager.Instance.InitiateBattle(character.gameObject, cell.occupant, () => ChangeState(AttackState.Finished));
                                yield return new WaitUntil(() => !BattleManager.Instance.IsBattleOngoing);
                                break; // Stop after attacking the first target
                            }
                        }
                    }
                    ChangeState(AttackState.Finished);
                }
                break;

            case AttackState.Finished:
                yield return new WaitForSeconds(0.5f);
                FinishedAI.Invoke();
                ChangeState(AttackState.Selection);
                break;
        }
    }

}
