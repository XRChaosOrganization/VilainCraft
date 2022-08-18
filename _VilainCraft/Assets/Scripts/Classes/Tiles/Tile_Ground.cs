using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile_Ground : Tile
{
    public bool isSlope;

    public Tile_Ground(Vector2 _gridPos, int _height)
    {
        type = Tile_Type.Ground;
        gridPos = _gridPos;
        height = _height;
        isBuildable = true;

        
    }
}
