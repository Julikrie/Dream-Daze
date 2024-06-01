using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    Selection,
    Movement,
    Finished
}
public class MovementStateMachine
{
    private MovementState currentState;
    // Cells I'm allowed to move in 
    private List<GridCell> allowedCells;
    private Character character;
    private MovementController movementController;
    private bool isEnabled;
    public event Action FinishedPlayer;
    public event Action FinishedAI;



    public MovementStateMachine(Character character, MovementController movementController)
    {
        this.character = character;
        this.movementController = movementController;
        // Is player turn active?
        isEnabled = false;
        currentState = MovementState.Selection;
        allowedCells = new List<GridCell>();
    }
    public void ChangeState(MovementState state)
    {
        currentState = state;
    }

    public void Update()
    {
        if (isEnabled)
        {
            if (TeamManager.Instance.IsPlayerCharacter(character))
            {
                HandlePlayerMovement();
            }
            else
            {

                // Use a Coroutine to wait for a certain period after highlighting tiles
                AICoroutineManager.Instance.RunCoroutine(HandleAIMovement());
            }

        }
    }
    public void Enable(bool isEnabled)
    {
        ChangeState(MovementState.Selection);
        this.isEnabled = isEnabled;
    }

    private void HandlePlayerMovement()
    {
        switch (currentState)
        {
            // Highlights the moveable areas
            case MovementState.Selection:
                allowedCells = TileSelector.Instance.HighlightMovementRange(character.transform.position, character.movementRange);
                ChangeState(MovementState.Movement);
                break;

            case MovementState.Movement:
                if (Input.GetMouseButtonDown(0))
                {
                    GridCell gridCell = GridManager.Instance.GetGridCellFromMousePosition();
                    if (gridCell != null && allowedCells.Contains(gridCell))
                    {
                        TileSelector.Instance.ClearMovementMarkers();
                        ActionUIManager.Instance.DisableMoveButton();
                        movementController.MoveTo(gridCell.position,() => ActionUIManager.Instance.SetCanvasActive(true));
                        ChangeState(MovementState.Finished);
                    }
                }
                ActionUIManager.Instance.ToggleCanvasOnClick();
                break;

            case MovementState.Finished:
                FinishedPlayer.Invoke();
                ChangeState(MovementState.Selection);
                break;
        }
    }

    private IEnumerator HandleAIMovement()
    {

        switch (currentState)
        {
            // Highlights the moveable areas
            case MovementState.Selection:
                allowedCells = TileSelector.Instance.HighlightMovementRange(character.transform.position, character.movementRange);
                yield return new WaitForSeconds(0.5f);
                ChangeState(MovementState.Movement);
                break;

            case MovementState.Movement:
                TileSelector.Instance.ClearMovementMarkers();
                ActionUIManager.Instance.DisableMoveButton();
                GridCell aiCurrentCell = GridManager.Instance.GetGridCell(character.transform.position);

                var attackableCells = Pathfinder.Instance.GetAttackableCells(aiCurrentCell, character.attackRange);


                GridCell nearestPlayerCell = GridManager.Instance.GetNearestPlayerCell(aiCurrentCell);
                if (nearestPlayerCell != null)
                {
                    GridCell targetCell = GridManager.Instance.GetClosestCellToPlayer(allowedCells, nearestPlayerCell);
                    if (targetCell != null)
                    {
                        bool hasMoved = false;
                        movementController.MoveTo(targetCell.position, () => hasMoved = true);
                        yield return new WaitUntil(() => hasMoved);
                    }
                }

                ChangeState(MovementState.Finished);
                break;

            case MovementState.Finished:
                FinishedAI.Invoke();
                ChangeState(MovementState.Selection);
                break;
        }
    }

}
