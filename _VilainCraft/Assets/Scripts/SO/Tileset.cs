using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_Tileset", menuName = "Data/Tileset",order = 0)]
public class Tileset : ScriptableObject
{
    public List<GameObject> terrain;
    public List<GameObject> water;
}
