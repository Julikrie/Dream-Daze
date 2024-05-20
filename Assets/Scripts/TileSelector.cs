using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    private List<GameObject> rangeMarkers = new List<GameObject>();
    public static TileSelector Instance { get; private set; }
    public GameObject indicatorPrefab, attackIndicatorPrefab;
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
            return gridCell;
        }
        return null;
    }
    // Highlights Characters movement range
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

    public List<GridCell> HighlightAttackRange(Vector3 startPosition, int attackRange)
    {
        var startCell = GridManager.Instance.GetGridCell(startPosition);
        var gridCells = Pathfinder.Instance.GetReachableCells(startCell, attackRange);
        foreach (var gridCell in gridCells)
        {
            {
                GameObject cell = Instantiate(attackIndicatorPrefab, GridManager.Instance.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
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







