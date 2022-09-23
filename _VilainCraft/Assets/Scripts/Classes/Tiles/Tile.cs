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
    
    

}
