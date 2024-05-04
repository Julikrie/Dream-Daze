using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public static TileSelector Instance { get; private set; }
    public Camera mainCamera;
    public GameObject indicatorPrefab;
    private GameObject selector;

    public MovementController movementController;
    public Pathfinder pathfinder;
    GridCell startCell = null;
    GridCell endCell = null;
    List<GameObject> pathMarkers = new List<GameObject>();
    List<GameObject> rangeMarkers = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            var gridPosition = GridManager.Instance.GetGridFromWorldPosition(mouseWorldPosition);
            if (GridManager.Instance.GetGridCell(gridPosition) != null)
            {
                movementController.MoveTo((Vector3Int)gridPosition);
            }
            //clearRangeMarkers();
        }*/


    }

    public GridCell HighlightIndicator(Vector3 worldPosition)
    {
        GridCell gridCell = GridManager.Instance.GetGridCell(worldPosition);
        if (gridCell != null)
        {

            if (selector != null)
            {
                Destroy(selector);
            }
            selector = Instantiate(indicatorPrefab, GridManager.Instance.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
            Debug.Log("Current cell(" + gridCell.position + ") is walkable:" + gridCell.isWalkable + ", has movement cost of " + gridCell.movementCost + " and is currently occupied:" + gridCell.occupant);
            return gridCell;
        }
        return null;
    }
    public void TestPathFinding(Vector3 worldPosition)
    {
        GridCell gridCell = GridManager.Instance.GetGridCell(worldPosition);

        if (startCell == null)
        {
            if (GridManager.Instance.IsWalkable(gridCell))
            {
                startCell = gridCell;
                Debug.Log("Start position set!");
            }
        }
        else if (endCell == null)
        {
            if (GridManager.Instance.IsWalkable(gridCell))
            {
                endCell = gridCell;
                Debug.Log("End position set! Calculating path...");

                List<GridCell> path = pathfinder.FindPath(startCell, endCell);
                Debug.Log($"{path.Count}");
                DrawPath(path);
            }
        }
        else
        {
            ClearPath();
            startCell = gridCell;
            endCell = null;
            Debug.Log("Start position reset!");
        }
    }
    void DrawPath(List<GridCell> path)
    {
        foreach (var gridCell in path)
        {
            GameObject pathMarker = Instantiate(indicatorPrefab, GridManager.Instance.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
            pathMarkers.Add(pathMarker);
        }
    }

    void ClearPath()
    {
        foreach (var marker in pathMarkers)
        {
            Destroy(marker);
        }
        pathMarkers.Clear();
    }
    // Displays the range Characters are allowed to walk
    public List<GridCell> HighlightMovementRange(Vector3 startPosition, int movementRange)
    {
        var startCell = GridManager.Instance.GetGridCell(startPosition);
        Vector2Int position = startCell.position;
        var gridCells = new List<GridCell>();
        for (int x = -movementRange; x <= movementRange; x++)
        {
            for (int y = -movementRange; y <= movementRange; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= movementRange)
                {
                    Vector2Int tilePosition = new Vector2Int(position.x + x, position.y + y);
                    var currentCell = GridManager.Instance.GetGridCell(new Vector3(tilePosition.x, tilePosition.y, 0));
                    if (Vector2Int.Distance(position, tilePosition) <= movementRange && GridManager.Instance.IsWalkable(currentCell))
                    {
                        GameObject cell = Instantiate(indicatorPrefab, GridManager.Instance.GetWorldFromCellPosition(new Vector2Int(tilePosition.x, tilePosition.y)) + Vector3.back, Quaternion.identity);
                        gridCells.Add(GridManager.Instance.GetGridCell(new Vector3(x, y, 0)));
                        rangeMarkers.Add(cell);
                    }
                }

            }
        }
        Debug.Log(gridCells.Count);
        return gridCells;
    }

    // clear highlighted fields
    public void clearRangeMarkers()
    {
        foreach (GameObject marker in rangeMarkers)
        {
            Destroy(marker);
        }
        rangeMarkers.Clear();
    }
}







