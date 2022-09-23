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

            //From the là

            if (MouseHandler.current.tile_mo != null && !GridManager.current.grid[MouseHandler.current.tile_mo.tileComponent.gridPos].building && r.materials[1] != wireframeGreen )
            {
                Material[] newMat = r.materials;
                newMat[1] = wireframeGreen;
                r.materials = newMat;
                r.materials[1].SetFloat("_isActive", 1f);
                canBuild = true;
            }

            if ((MouseHandler.current.tile_mo == null || GridManager.current.grid[MouseHandler.current.tile_mo.tileComponent.gridPos].building) && r.materials[1] != wireframeRed)
            {
                Material[] newMat = r.materials;
                newMat[1] = wireframeRed;
                r.materials = newMat;
                r.materials[1].SetFloat("_isActive", 1f);
                canBuild = false;
            }

            //To the là         il va falloir remanier tout ça, le faire proprement, mettre le check is buildable ailleurs (genre GridUtilities) et avoir des conditions détaillées. Si une seule n'est pas remplie
            //                  on passe rouge et on peut pas construire, l'idée c'est d'avoir un rapport des differentes raisons pour lesquelles la construction est impossible, et de les display a coté du
            //                  curseur dans une infobulle.
            //                  
            //                  Pour ça, attendre d'avoir un systeme de tile necessaire pour la construction pour chaque tile occupée par le prefab. Doit etre compatible avec la rotation des buildings



            buildingPreview.transform.position = MouseHandler.current.tile_mo != null ? MouseHandler.current.tile_mo.tileAnchor : MouseHandler.current.screenPoint;
            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                Instantiate(buildingStamp, MouseHandler.current.tile_mo.tileAnchor, Quaternion.identity, buildingContainer);
                GridManager.current.grid[MouseHandler.current.tile_mo.tileComponent.gridPos].building = buildingStamp;
            }

            
        }
    }




}
