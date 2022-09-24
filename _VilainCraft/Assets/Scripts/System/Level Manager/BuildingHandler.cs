using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public static BuildingHandler current;

    public bool AlwaysDisplayGrid { get { return alwaysDisplayGrid; } set { alwaysDisplayGrid = value; GameEvents.current.DisplayGrid(value || isBuildMode); } }
    bool alwaysDisplayGrid;
    public bool IsBuildMode {get {return isBuildMode;} set { isBuildMode = value; GameEvents.current.DisplayGrid(value || alwaysDisplayGrid);}}
    bool isBuildMode;

    public Transform buildingContainer;
    public GameObject buildingStamp;
    public Building buildData;
    List<Pos_Type_Pair> nonVoidTiles;
    Renderer r;
    public Material wireframeGreen;
    public Material wireframeRed;
    bool canBuild;

    GameObject buildingPreview;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        HandleBuildingStamp();
    }

    public void SetBuildingStamp(GameObject _building)
    {
        buildingStamp = _building;
        buildData = buildingStamp.GetComponent<BuildingBehavior>().building;

        nonVoidTiles = new List<Pos_Type_Pair>();
        foreach (var tile in buildData.requiredTiles)
            if (tile.tileType != Tile.Tile_Type.Void)
                nonVoidTiles.Add(tile);

        Transform model = _building.transform.Find("Model");
        buildingPreview = Instantiate(model.gameObject, MouseHandler.current.screenPoint, Quaternion.identity, transform);
        r = buildingPreview.GetComponent<Renderer>();
        Color c = new Color(1f, 1f, 1f, 0.5f);
        r.materials[0].color = c;

        IsBuildMode = true;
    }

    void HandleBuildingStamp()
    {
        if (IsBuildMode && buildingStamp != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(buildingPreview);
                buildingPreview = null;
                buildingStamp = null;
                IsBuildMode = false;
                return;
            }


            if (BuildingCanBePlaced() && r.materials[1] != wireframeGreen )
            {
                Material[] newMat = r.materials;
                newMat[1] = wireframeGreen;
                r.materials = newMat;
                r.materials[1].SetFloat("_isActive", 1f);
                canBuild = true;
            }
            if (!BuildingCanBePlaced() && r.materials[1] != wireframeRed)
            {
                Material[] newMat = r.materials;
                newMat[1] = wireframeRed;
                r.materials = newMat;
                r.materials[1].SetFloat("_isActive", 1f);
                canBuild = false;
            }


            buildingPreview.transform.position = MouseHandler.current.tile_mo != null ? MouseHandler.current.tile_mo.tileAnchor : MouseHandler.current.screenPoint;


            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                Instantiate(buildingStamp, MouseHandler.current.tile_mo.tileAnchor, Quaternion.identity, buildingContainer);
                foreach (var pair in nonVoidTiles)
                    GridManager.current.grid[MouseHandler.current.tile_mo.tileComponent.gridPos + pair.gridPos].building = buildingStamp;

            }
        }
    }


    // Add a rotate method, called by new Input system, methods only rotate the vector2 matrix (non void tiles) to match building facing direction. No need to do anymore, rotating the matrix will affect the conditions for placing the building automatically;


    bool BuildingCanBePlaced()
    {
        if (MouseHandler.current.tile_mo == null)
            return false;


        foreach (var tile in nonVoidTiles)
        {
            Vector2 targetTile = MouseHandler.current.tile_mo.tileComponent.gridPos + tile.gridPos;
            if (!GridManager.current.grid.ContainsKey(targetTile))
                return false;

            if (GridManager.current.grid[targetTile].height != MouseHandler.current.tile_mo.tile.height)
                return false;

            if (GridManager.current.grid[targetTile].type != tile.tileType  || GridManager.current.grid[targetTile].building != null)
                return false;
        }

        return true;
    }




}
