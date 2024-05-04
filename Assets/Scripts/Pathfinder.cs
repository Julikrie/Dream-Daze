using System.Collections.Generic;
using UnityEngine;


public class Pathfinder : MonoBehaviour
{
     public List<GridCell> FindPath(GridCell startCell, GridCell targetCell)
    {
        List<GridCell> openSet = new List<GridCell>();
        HashSet<GridCell> closedSet = new HashSet<GridCell>();
        ResetPathfindingData();
        startCell.gCost = 0;
        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            GridCell currentCell = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentCell.fCost || (openSet[i].fCost == currentCell.fCost && openSet[i].hCost < currentCell.hCost))
                {
                    currentCell = openSet[i];
                }
            }

            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            if (currentCell == targetCell)
            {
                return RetracePath(startCell, targetCell);
            }

            foreach (GridCell neighbor in GridManager.Instance.GetNeighbors(currentCell))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    continue;
                Debug.Log(neighbor);
                int newMovementCostToNeighbor = currentCell.gCost + neighbor.movementCost;
                if (newMovementCostToNeighbor < neighbor.gCost)
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetCell);
                    neighbor.parent = currentCell;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        Debug.Log("bin hier");
        return new List<GridCell>(); // Return an empty path if there's no way to reach the target
    }

    private List<GridCell> RetracePath(GridCell startCell, GridCell endCell)
    {
        List<GridCell> path = new List<GridCell>();
        GridCell currentCell = endCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.parent;
        }
        path.Reverse();

        return path;
    }

    private int GetDistance(GridCell cellA, GridCell cellB)
    {
        int distX = Mathf.Abs(cellA.position.x - cellB.position.x);
        int distY = Mathf.Abs(cellA.position.y - cellB.position.y);
        return 14 * (distX + distY) - 6 * Mathf.Min(distX, distY);
    }

    public void ResetPathfindingData()
    {
        foreach (var cell in GridManager.Instance.grid.Values)  // Assuming 'grid' is accessible like this
        {
            cell.gCost = int.MaxValue;
            cell.parent = null;
            cell.hCost = 0; // Reset hCost if it's not recalculated for each new path
        }
    }
}
