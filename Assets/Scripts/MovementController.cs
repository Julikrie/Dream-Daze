using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public GridManager gridManager;
    public Pathfinder pathfinder;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();

    void Update()
    {
        // Only trigger movement on a specific condition (like a key press or mouse click)
        // Here's how you might handle it with a key press:
        if (Input.GetKeyDown(KeyCode.M) && pathPoints.Count == 0)  // Example: Press 'M' to move
        {
            MoveTo(new Vector3Int(-8, 0, -1));  // Only call this once per key press
        }

        // Always call MoveAlongPath to continue moving along the current path
        MoveAlongPath();
    }

    public void MoveTo(Vector3Int targetGridPosition)
    {
        if (pathPoints.Count == 0) // Check if there's already a path being followed
        {
            GridCell startCell = gridManager.GetGridCell(transform.position);
            GridCell targetCell = gridManager.GetGridCell(targetGridPosition);

            if (startCell != null && targetCell != null) // Make sure the cells are valid
            {
                Debug.Log($"Attempting to move from {startCell.position} (Walkable: {startCell.isWalkable}) to {targetCell.position} (Walkable: {targetCell.isWalkable})");

                List<GridCell> path = pathfinder.FindPath(startCell, targetCell);
                Debug.Log($"Path length: {path.Count}");
                if (path.Count > 0)
                {
                    SetPath(path);
                }
            }
        }
    }



    private void SetPath(List<GridCell> path)
    {
        Debug.Log($"Setting path with {path.Count} points.");
        pathPoints.Clear();
        foreach (var cell in path)
        {
            Vector3 worldPos = gridManager.GetWorldFromCellPosition(cell);
            Debug.Log($"Adding path point at world position: {worldPos}");
            pathPoints.Enqueue(worldPos);
        }
        MoveAlongPath();  // Start moving immediately
    }


    private void MoveAlongPath()
    {
        if (pathPoints.Count > 0)
        {
            Vector3 targetPosition = pathPoints.Peek();
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                transform.position += moveDirection * 5 * Time.deltaTime;
            }
            else
            {
                transform.position = targetPosition;
                pathPoints.Dequeue();
                Debug.Log($"Reached {targetPosition}, remaining points: {pathPoints.Count}");
            }
        }
    }
}

