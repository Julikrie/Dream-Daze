public class TileAttributes
{
    public bool isWalkable;
    public int movementCost;

    public TileAttributes(bool isWalkable, int movementCost)
    {
        this.isWalkable = isWalkable;
        this.movementCost = movementCost;
    }
}

