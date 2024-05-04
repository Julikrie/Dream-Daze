using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public Pathfinder pathfinder;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();

    void Update()
    {
        MoveAlongPath();
    }

    public void MoveTo(Vector3Int targetGridPosition, List<GridCell> allowedCells)
    {
        // Check if Path is already followed
        if (pathPoints.Count == 0)
        {
            GridCell startCell = GridManager.Instance.GetGridCell(transform.position);
            GridCell targetCell = GridManager.Instance.GetGridCell(targetGridPosition);

            // Make sure the cells are valid
            if (startCell != null && targetCell != null && allowedCells.Contains(targetCell))
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
            Vector3 worldPos = GridManager.Instance.GetWorldFromCellPosition(cell);
            Debug.Log($"Adding path point at world position: {worldPos}");
            pathPoints.Enqueue(worldPos);
        }
        MoveAlongPath();
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

