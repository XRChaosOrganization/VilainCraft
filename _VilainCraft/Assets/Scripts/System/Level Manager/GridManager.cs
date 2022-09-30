using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{

    public static GridManager current;
    public List<Pos_Tile_Pair> gridData = new List<Pos_Tile_Pair>();
    public Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();

    
    

    private void Awake()
    {
        current = this;
        LoadGrid();
    }
    
    public void SaveGrid()
    {
        gridData.Clear();
        foreach (KeyValuePair<Vector2, Tile> pair in grid)
            gridData.Add(new Pos_Tile_Pair(pair.Key, pair.Value));
    }

    public void LoadGrid()
    {
        foreach (Pos_Tile_Pair pair in gridData)
            grid[pair.gridPos] = pair.tile;

    }
    
}
