using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public static BuildingHandler current;
 
    public bool IsBuildMode {get {return isBuildMode;} set { isBuildMode = value; GameEvents.current.DisplayGrid(value || alwaysDisplayGrid);}}
    bool isBuildMode;
    public bool AlwaysDisplayGrid { get { return alwaysDisplayGrid; } set { alwaysDisplayGrid = value; GameEvents.current.DisplayGrid(value || isBuildMode); } }
    bool alwaysDisplayGrid;



}
