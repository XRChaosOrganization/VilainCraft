using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapEngine;


[CreateAssetMenu(fileName = "", menuName = "Game Data/Building", order = 1)]
public class Building : ScriptableObject
{
    public int cost;
    public Sprite sprite;
    [HideInInspector] public List<Pos_Type_Pair> requiredTiles = new List<Pos_Type_Pair>();
}


