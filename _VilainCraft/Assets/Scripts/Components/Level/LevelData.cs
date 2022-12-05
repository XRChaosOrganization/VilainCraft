using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelData : MonoBehaviour
{
    public static LevelData current;
    public List<Tile> serializableGrid = new List<Tile>();
    public Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();





    private void Awake()
    {
        current = this;
        LoadGrid();
    }

    public void SaveGrid()
    {
        serializableGrid.Clear();
        foreach (KeyValuePair<Vector2, Tile> pair in grid)
            serializableGrid.Add(new Tile(pair.Key, pair.Value.type,  pair.Value.height));
            
    }

    public void LoadGrid()
    {
        foreach (Tile tile in serializableGrid)
            grid[tile.gridPos] = tile;
        

        

    }
}
