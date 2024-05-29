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
    public event Action Finished;

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
                            movementController.MoveTo(gridCell.position);
                            ChangeState(MovementState.Finished);
                        }
                    }
                    break;

                case MovementState.Finished:
                    Finished.Invoke();
                    ChangeState(MovementState.Selection);
                    break;
            }
        }
    }
    public void Enable(bool isEnabled)
    {
        ChangeState(MovementState.Selection);
        this.isEnabled = isEnabled;
    }

}