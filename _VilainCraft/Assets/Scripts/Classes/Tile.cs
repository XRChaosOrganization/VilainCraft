using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class Tile
{
    public enum Tile_Type { Void, Grass, Water };

    public Tile_Type type;
    public Vector2 gridPos;
    public int height;
    public bool isBlocked;
    public RawRessource node;
    public GameObject building;
    public GameObject associatedGO;

    public Tile(Vector2 _gridPos, Tile_Type _type,  int _height = 0, bool _isBlocked = false)
    {
        type = _type;
        gridPos = _gridPos;
        height = _height;
        isBlocked = _isBlocked;

    }




#if UNITY_EDITOR


    [HideInInspector] public bool waterfallBlock;

#endif

}
