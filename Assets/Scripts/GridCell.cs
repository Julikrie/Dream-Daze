using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public Vector2Int position;
    public GameObject occupant;
    public bool isWalkable;
    public int movementCost;


    public int gCost = int.MaxValue;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }
    public GridCell parent;

    
    public GridCell(Vector2Int position, GameObject occupant, bool isWalkable, int movementCost)
    {
        this.position = position;
        this.occupant = occupant;
        this.isWalkable = isWalkable;
        this.movementCost = movementCost;
    }   
}
