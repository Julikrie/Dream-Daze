using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Pathfinder : MonoBehaviour
{
    public static Pathfinder Instance {get; private set;}
    private void Awake()
    {   // Destroy Pathfinder GameObjects if there is already one, otherwise create it
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // A* Pathfinding algorithm
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
            // Explores Cells for shortest Way
            if (currentCell == targetCell)
            {
                return RetracePath(startCell, targetCell);
            }

            foreach (GridCell neighbor in GridManager.Instance.GetNeighbors(currentCell))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    continue;
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
        // return empty Path if not reachable
        Debug.Log("bin hier");
        return new List<GridCell>();
    }

    // Look at every Cell the Character can walk and provide them
    public List<GridCell> GetReachableCells(GridCell startCell, int maxRange)
    {
        Queue<GridCell> queue = new Queue<GridCell>();
        Dictionary<GridCell, int> costSoFar = new Dictionary<GridCell, int>();
        queue.Enqueue(startCell);
        costSoFar[startCell] = 0;
        List<GridCell> reachableCells = new List<GridCell>();
        while (queue.Count > 0)
        {
            GridCell current = queue.Dequeue();
            foreach (GridCell neighbor in GridManager.Instance.GetNeighbors(current))
            {
                if (!GridManager.Instance.IsWalkable(neighbor))
                    continue;
                int newCost = costSoFar[current] + neighbor.movementCost;
                if (newCost > maxRange)  
                    continue;
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    queue.Enqueue(neighbor);
                    reachableCells.Add(neighbor); 
                }
            }
        }
        return reachableCells;
    }
    // Returns Cells representing Path
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
        foreach (var cell in GridManager.Instance.grid.Values)
        {
            cell.gCost = int.MaxValue;
            cell.parent = null;
            cell.hCost = 0; 
        }
    }
    public List<GridCell> GetAttackableCells(GridCell startCell, int attackRange)
    {
        List<GridCell> attackableCells = new List<GridCell>();

        for (int x = -attackRange; x <= attackRange; x++)
        {
            for (int y = -attackRange; y <= attackRange; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= attackRange)
                {
                    GridCell currentCell = GridManager.Instance.GetGridCell(new Vector2(startCell.position.x + x, startCell.position.y + y));
                    attackableCells.Add(currentCell);
                }
            }
        }
        return attackableCells;
    }
}
