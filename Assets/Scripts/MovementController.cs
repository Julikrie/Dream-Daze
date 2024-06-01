using System;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    private Action onMoveCompleted;

    void Update()
    {
        MoveAlongPath();
    }
    public void MoveTo(Vector2Int targetGridPosition, Action onMoveCompleted = null)
    {
        // Check if Path is already followed
        if (pathPoints.Count == 0)
        {
            GridCell startCell = GridManager.Instance.GetGridCell(transform.position);
            GridCell targetCell = GridManager.Instance.GetGridCell(targetGridPosition);

            // Make sure the cells are valid
            if (startCell != null && targetCell != null)
            {
                List<GridCell> path = Pathfinder.Instance.FindPath(startCell, targetCell);
                if (path.Count > 0)
                {
                    SetPath(path);
                }
                // Looking if someone is on the Cell
                // Move Object from start Cell to target Cell
                startCell.occupant = null;
                targetCell.occupant = gameObject;

                // Invoke the callback if provided
                this.onMoveCompleted = onMoveCompleted;
            }
        }
    }
    // Setting Path 
    private void SetPath(List<GridCell> path)
    {
        pathPoints.Clear();
        foreach (var cell in path)
        {
            Vector3 worldPos = GridManager.Instance.GetWorldFromCellPosition(cell);
            pathPoints.Enqueue(worldPos);
        }
        MoveAlongPath();
    }
    // Walking the Path
    private void MoveAlongPath()
    {
        // As long as there are Tiles to walk, walk to the next Tile
        if (pathPoints.Count > 0)
        {
            // Look at next Tile
            Vector3 targetPosition = pathPoints.Peek();
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                // Move to next Tile
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                transform.position += moveDirection * 5 * Time.deltaTime;
            }
            else
            {
                // When close enough set to exact position 
                transform.position = targetPosition;
                pathPoints.Dequeue();
            }
        }
        else if (onMoveCompleted != null)
        {
            // Invoke the callback when the path is completed
            onMoveCompleted.Invoke();
            // Reset the callback after invoking
            onMoveCompleted = null;
        }
    }
}


