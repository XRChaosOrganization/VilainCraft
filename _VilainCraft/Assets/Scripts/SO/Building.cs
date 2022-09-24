using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "", menuName = "Building", order = 1)]
public class Building : ScriptableObject
{
    public int cost;
    public Sprite sprite;
    [HideInInspector] public List<Pos_Type_Pair> requiredTiles = new List<Pos_Type_Pair>();
}

#region Custom Editor

[System.Serializable]
public class Pos_Type_Pair
{
    public Vector2 gridPos;
    public Tile.Tile_Type tileType;
    public Pos_Type_Pair(Vector2 _pos, Tile.Tile_Type _tileType)
    {
        gridPos = _pos;
        tileType = _tileType;
    }
}

#endregion
