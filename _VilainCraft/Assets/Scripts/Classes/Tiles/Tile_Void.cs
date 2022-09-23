using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile_Void : Tile
{
    public Tile_Void(Vector2 _gridPos, int _height = 0)
    {
        type = Tile_Type.Void;
        gridPos = _gridPos;
        height = _height;

    }
}
