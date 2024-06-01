using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public Tilemap tileMap;
    public TileAttributesManager tileAttributesManager;
    public Camera mainCamera;

    // Dictionary with int Vector as key, cell information X,Y as value
    public Dictionary<Vector2Int, GridCell> grid;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            //Generate Grid before everything else to position Characters
            grid = new Dictionary<Vector2Int, GridCell>();
            generateGrid();
        }
    }

    // Generate Grid for game field 
    void generateGrid()
    {
        for (int x = tileMap.cellBounds.min.x; x < tileMap.cellBounds.max.x; x++)
        {
            for (int y = tileMap.cellBounds.min.y; y < tileMap.cellBounds.max.y; y++)
            {
                TileAttributes tile = tileAttributesManager.GetTileAttributes(x, y);
                if (tile != null)
                {
                    Vector2Int currentCell = new Vector2Int(x, y);
                    // Adds each cell to the grid dictionary
                    grid.Add(currentCell, new GridCell(currentCell, null, tile.isWalkable, tile.movementCost));
                }
            }
        }
    }

    public GridCell GetGridCell(Vector2 gridPosition)
    {
        var cellPosition = Vector2Int.RoundToInt(gridPosition);
        if (grid.ContainsKey(cellPosition))
        {
            return grid[cellPosition];
        }
        return null;
    }

    public GridCell GetGridCell(Vector3 worldPosition)
    {
        // Convert from World to grid coordinates to get cell information
        Vector3Int tilePosition = tileMap.WorldToCell(worldPosition);
        Vector2Int cellPosition = new Vector2Int(tilePosition.x, tilePosition.y);
        // Look if in Dictionary
        if (grid.ContainsKey(cellPosition))
        {
            return grid[cellPosition];
        }
        return null;
    }

    // Get mouse position directly
    public GridCell GetGridCellFromMousePosition()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        return GetGridCell(mouseWorldPosition);
    }

    public Vector3 GetWorldFromCellPosition(Vector2Int cellPosition)
    {
        // Convert from Grid to World coordinates
        Vector3 worldPosition = tileMap.CellToWorld((Vector3Int)cellPosition);
        // Get Center of Tile
        return worldPosition + new Vector3(tileMap.cellSize.x / 2, tileMap.cellSize.y / 2, 0);
    }


    public Vector3 GetWorldFromCellPosition(GridCell cell)
    {
        return GetWorldFromCellPosition(cell.position);
    }

    public Vector2Int GetGridFromWorldPosition(Vector3 worldPosition)
    {
        Vector3Int tilePosition = tileMap.WorldToCell(worldPosition);
        return (Vector2Int)tilePosition;
    }

    public List<GridCell> GetGridCellsOfPlayerCharacters()
    {
        List<GridCell> playerCharacterCells = new List<GridCell>();

        foreach (Character playerCharacter in TeamManager.Instance.playerTeam)
        {
            GameObject parentObject = playerCharacter.gameObject;

            GridCell cell = GetGridCell(parentObject.transform.position);
            if (cell != null && !playerCharacterCells.Contains(cell))
            {
                playerCharacterCells.Add(cell);
            }
        }

        return playerCharacterCells;
    }
    public GridCell GetNearestPlayerCell(GridCell aiCell)
    {
        //Get the nearest Player to calculate to whom to move
        GridCell nearestPlayerCell = null;
        float minDistance = float.MaxValue;

        foreach (GridCell playerCell in GetGridCellsOfPlayerCharacters())
        {
            float distance = Pathfinder.Instance.GetDistance(aiCell, playerCell);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayerCell = playerCell;
            }
        }

        return nearestPlayerCell != null ? nearestPlayerCell : null;
    }

    public GridCell GetClosestCellToPlayer(List<GridCell> allowedCells, GridCell targetCell)
    {
        //Get the closest cell to the nearest player within the possible movement range
        GridCell nearestAllowedCell = null;
        float minDistance = float.MaxValue;

        foreach (GridCell allowedCell in allowedCells)
        {
            float distance = Pathfinder.Instance.GetDistance(allowedCell, targetCell);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestAllowedCell = allowedCell;
            }
        }

        return nearestAllowedCell;
    }

    public bool IsWalkable(GridCell cellCoords)
    {
        return cellCoords != null && cellCoords.isWalkable && cellCoords.occupant == null;
    }
    public List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new List<GridCell>();

        // Definition of every direction
        Vector2Int[] directions = new Vector2Int[]
        {
        // Up    
        new Vector2Int(0, 1),
        // Right
        new Vector2Int(1, 0),
        // Down
        new Vector2Int(0, -1),
        // Left
        new Vector2Int(-1, 0)
        };

        // Can move to Neighbor Cell if free and walkable
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = new Vector2Int(cell.position.x + direction.x, cell.position.y + direction.y);
            GridCell neighbor = GetGridCell(neighborPos);
            if (neighbor != null && neighbor.isWalkable)
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }


}
