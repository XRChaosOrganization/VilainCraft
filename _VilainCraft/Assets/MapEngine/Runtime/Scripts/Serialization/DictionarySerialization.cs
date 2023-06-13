using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEngine
{
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

    [System.Serializable]
    public class Pos_Tile_Pair
    {
        public Vector2 gridPos;
        public Tile tile;
        public Pos_Tile_Pair(Vector2 _pos, Tile _tile)
        {
            gridPos = _pos;
            tile = _tile;
        }
    }

}
