using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile_Water : Tile
{
    public Tile_Water(Vector2 _gridPos, int _height)
    {
        type = Tile_Type.Water;
        gridPos = _gridPos;
        height = _height;
        isBuildable = true;
    }
}
