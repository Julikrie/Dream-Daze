using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    private List<GameObject> movementMarkers = new List<GameObject>();
    private List<GameObject> attackMarkers = new List<GameObject>();
    public static TileSelector Instance { get; private set; }
    public GameObject indicatorPrefab, attackIndicatorPrefab;
    private GameObject selector;

    private bool paused;

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

    // Highlights Characters movement range
    public List<GridCell> HighlightMovementRange(Vector3 startPosition, int movementRange)
    {
        ClearMovementMarkers();
        var startCell = GridManager.Instance.GetGridCell(startPosition);
        var gridCells = Pathfinder.Instance.GetReachableCells(startCell, movementRange);
        foreach (var gridCell in gridCells)
        {
            if (gridCell != startCell && GridManager.Instance.IsWalkable(gridCell))
            {
                GameObject cell = Instantiate(indicatorPrefab, GridManager.Instance.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
                movementMarkers.Add(cell);
            }
        }
        return gridCells;
    }

    // Highlights Characters attack range
    public List<GridCell> HighlightAttackRange(Vector3 startPosition, int attackRange)
    {
        ClearAttackMarkers();
        var startCell = GridManager.Instance.GetGridCell(startPosition);
        var gridCells = Pathfinder.Instance.GetAttackableCells(startCell, attackRange);
        foreach (var gridCell in gridCells)
        {
            if (gridCell != startCell && gridCell != null)
            {
                GameObject cell = Instantiate(attackIndicatorPrefab, GridManager.Instance.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
                attackMarkers.Add(cell);
            }
        }
        return gridCells;
    }

    // clear highlighted Cells
    public void ClearMovementMarkers()
    {
        foreach (GameObject marker in movementMarkers)
        {
            Destroy(marker);
        }
        movementMarkers.Clear();
    }

    //clear highlighted attack cells
    public void ClearAttackMarkers()
    {
        foreach (GameObject marker in attackMarkers)
        {
            Destroy(marker);
        }
        attackMarkers.Clear();
    }
}