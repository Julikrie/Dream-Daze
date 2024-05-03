using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject indicatorPrefab;
    public GridManager gridManager;
    private GameObject selector;

    public MovementController movementController;
    public Pathfinder pathfinder;
    GridCell startCell = null;
    GridCell endCell = null;
    List<GameObject> pathMarkers = new List<GameObject>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)); // Adjust z based on your camera setup
            var gridPosition = gridManager.GetGridFromWorldPosition(mouseWorldPosition); // Convert world position to grid position
            if (gridManager.GetGridCell(gridPosition) != null) // Ensure this is a valid position
            {
                movementController.MoveTo((Vector3Int)gridPosition);
            }
        }


    }


    public GridCell HighlightIndicator(Vector3 worldPosition)
    {
        GridCell gridCell = gridManager.GetGridCell(worldPosition);
        if (gridCell != null)
        {

            if (selector != null)
            {
                Destroy(selector);
            }
            selector = Instantiate(indicatorPrefab, gridManager.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
            Debug.Log("Current cell(" + gridCell.position + ") is walkable:" + gridCell.isWalkable + ", has movement cost of " + gridCell.movementCost + " and is currently occupied:" + gridCell.occupant);
            return gridCell;
        }
        return null;
    }
    public void TestPathFinding(Vector3 worldPosition)
    {
        GridCell gridCell = gridManager.GetGridCell(worldPosition);

        if (startCell == null)
        {
            if (gridManager.IsWalkable(gridCell))
            {
                startCell = gridCell;
                Debug.Log("Start position set!");
            }
        }
        else if (endCell == null)
        {
            if (gridManager.IsWalkable(gridCell))
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
            GameObject pathMarker = Instantiate(indicatorPrefab, gridManager.GetWorldFromCellPosition(gridCell) + Vector3.back, Quaternion.identity);
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

}


