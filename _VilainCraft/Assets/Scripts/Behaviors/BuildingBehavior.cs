using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehavior : MonoBehaviour
{
    public Building building;


    #region Unity Loop
    void Start()
    {
        foreach (Pos_Type_Pair pair in building.requiredTiles)
        {
            if (pair.tileType != Tile.Tile_Type.Void)
                Debug.Log(pair.tileType);
        }
    }

    void Update()
    {
        
    }

    #endregion




    #region Virtuals


    //Will contain generic virtual methods




    #endregion
}
