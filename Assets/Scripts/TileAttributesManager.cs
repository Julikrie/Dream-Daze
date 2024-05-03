using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileAttributesManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<TileType, TileAttributes> mapping;

    private void Awake()
    {   // Set Attributes for each Tile Type
        mapping = new Dictionary<TileType, TileAttributes>
        {
            {TileType.Walkable, new TileAttributes(true, 1)},
            {TileType.Difficult, new TileAttributes(true, 5)},
            {TileType.Impassable, new TileAttributes(false, 0)}
        };
    }

    public TileAttributes GetTileAttributes(int x, int y)
    {
        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
        if (tile != null)
        {
            switch (tile.name)
            {
                case string name when name.Contains("Walkable"):
                    return mapping[TileType.Walkable];
                case string name when name.Contains("Difficult"):
                    return mapping[TileType.Difficult];
                case string name when name.Contains("Impassable"):
                    return mapping[TileType.Impassable];
                default:
                    return mapping[TileType.Walkable];
            }
        }
        return null;
    }
}

