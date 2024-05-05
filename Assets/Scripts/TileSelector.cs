using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    private List<GameObject> rangeMarkers = new List<GameObject>();
    public static TileSelector Instance { get; private set; }
    public GameObject indicatorPrefab;
    private GameObject selector;

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
        HighlightIndicator();
    }

    public GridCell HighlightIndicator()
    {   // Highlighting the Cell hovered over
        GridCell gridCell = GridManager.Instance.GetGridCellFromMousePosition();
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
    public List<GridCell> HighlightMovementRange(Vector3 startPosition, int movementRange)
    {
        var startCell = GridManager.Instance.GetGridCell(startPosition);
        var gridCells = Pathfinder.Instance.GetReachableCells(startCell, movementRange);
        foreach (var gridCell in gridCells)
        {
            if (GridManager.Instance.IsWalkable(gridCell))
            {
                GameObject cell = Instantiate(indicatorPrefab, GridManager.Instance.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
                rangeMarkers.Add(cell);
            }
        }
        return gridCells;
    }
    // clear highlighted Cells
    public void clearRangeMarkers()
    {
        foreach (GameObject marker in rangeMarkers)
        {
            Destroy(marker);
        }
        rangeMarkers.Clear();
    }
}







