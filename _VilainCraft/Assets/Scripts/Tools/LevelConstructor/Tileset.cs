using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Waterfall_Tileset
{
    public List<GameObject> straight;
    public List<GameObject> cornerIn;
    public List<GameObject> cornerOut;
}


[CreateAssetMenu(fileName = "_Tileset", menuName = "Data/Tileset",order = 0)]

public class Tileset : ScriptableObject
{
    public List<GameObject> ground;
    public List<GameObject> groundVariants;
    public List<GameObject> water;
    public Waterfall_Tileset waterfall;



}

