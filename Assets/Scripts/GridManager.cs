using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tileMap;
    private Dictionary<Vector2, GameObject> grid;
    // Start is called before the first frame update
    void Start()
    {

        generateGrid();
    }
    
    // Grid vom Spielfeld generieren
    void generateGrid()
    {
        for (int x = tileMap.cellBounds.min.x; x < tileMap.cellBounds.max.x; ++x)
        {
            for (int y = tileMap.cellBounds.min.y; y < tileMap.cellBounds.max.y; ++y)
            {
                grid.Add(new Vector2(x, y), null);
                Debug.DrawLine(new Vector2(x,y), new Vector2(x+1, y), Color.red, 10000f);
                Debug.DrawLine(new Vector2(x, y), new Vector2(x, y+1), Color.red, 10000f);
            }
        }

    }
    Vector2 getTileFromWorldCoordinate(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition)
    }
}
