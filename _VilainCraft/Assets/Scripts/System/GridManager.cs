using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Pos_Type_Pair
{
    public Vector2 gridPos;
    public Tile tile;
    public Pos_Type_Pair(Vector2 _pos, Tile _tile)
    {
        gridPos = _pos;
        tile = _tile;
    }
}

public class GridManager : MonoBehaviour
{

    public static GridManager current;
    public List<Pos_Type_Pair> gridData = new List<Pos_Type_Pair>();
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
            gridData.Add(new Pos_Type_Pair(pair.Key, pair.Value));
    }

    public void LoadGrid()
    {
        foreach (Pos_Type_Pair pair in gridData)
            grid[pair.gridPos] = pair.tile;

    }
    
}
