using UnityEngine;

public class GridCell
{
    public Vector2Int position;
    public GameObject occupant;
    public bool isWalkable;
    public int movementCost;

    // Cost of the path to Grid Cell
    public int gCost;
    // Estimated Cost of target destination
    public int hCost;
    // Sum of gCost + hCost
    public int fCost { get { return gCost + hCost; } }
    public GridCell parent;

    
    public GridCell(Vector2Int position, GameObject occupant, bool isWalkable, int movementCost)
    {
        this.position = position;
        this.occupant = occupant;
        this.isWalkable = isWalkable;
        this.movementCost = movementCost;
    }   
    public Character GetCharacter()
    {
        if (occupant != null)
        {
            return occupant.GetComponent<Character>();
        }
        return null;
    }
}
