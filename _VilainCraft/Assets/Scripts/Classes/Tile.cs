using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class Tile
{
    public enum Tile_Type { Void, Ground, Water };

    public Tile_Type type;
    public Vector2 gridPos;
    public int height;
    public GameObject building;
    public GameObject associatedGO;

    public Tile(Tile_Type _type, Vector2 _gridPos, int _height = 0)
    {
        type = _type;
        gridPos = _gridPos;
        height = _height;

    }

}
