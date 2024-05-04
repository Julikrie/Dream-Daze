using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateMachine : MonoBehaviour
{
    public Camera mainCamera;
    private MovementState currentState;
    // Cells I'm allowed to move in 
    private List<GridCell> allowedCells;

    private bool showSelection;
    public void Start()
    {
        currentState = MovementState.Selection;
        showSelection = true;
        allowedCells = new List<GridCell>();
    }

    public void ChangeState(MovementState state)
    {
        currentState = state;
    }

    public void Update()
    {
        switch (currentState)
        {
            case MovementState.Selection:
                if (showSelection)
                {
                    allowedCells = TileSelector.Instance.HighlightMovementRange(transform.position, 3);
                    showSelection = false;  // Ensure this only happens once per entry into this state
                }

                // Wait for a user input to change state, should not automatically transition to Movement state.
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeState(MovementState.Movement);
                }
                break;

            case MovementState.Movement:
                if (Input.GetMouseButtonDown(0))
                {
                    TileSelector.Instance.clearRangeMarkers();
                    Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    var gridPosition = GridManager.Instance.GetGridFromWorldPosition(mouseWorldPosition);
                    if (GridManager.Instance.GetGridCell(gridPosition) != null)
                    {
                        Debug.Log(allowedCells.Count);
                        GetComponent<MovementController>().MoveTo((Vector3Int)gridPosition, allowedCells);
                    }
                    ChangeState(MovementState.Finished);
                }
                break;

            case MovementState.Finished:
                    showSelection = true;
                    ChangeState(MovementState.Selection);

                break;
        }
    }
}
