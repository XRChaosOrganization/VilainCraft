using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Camera cam;
    public GameObject TbuildingPrefab;
    public GameObject building3x1Prefab;
    public GameObject building2x2Prefab;
    public Transform currentSelectedTile;
    public GameObject currentSelectedBuilding;
    public GameObject buildingPreview;
    public static UIManager current;
    public bool isBuilding ;

    private void Awake()
    {
        cam = Camera.main;
        current = this;
    }
    private void Update()
    {

        // CHANGER SYSTEME D'INSTANCIATION !!!!!!
        if (isBuilding && currentSelectedTile != null)
        {
            if (buildingPreview.GetComponent<BuildingComponent>().buildable == true)
            {
                for (int i = 0; i < buildingPreview.GetComponent<BuildingComponent>().buildingMeshs.Count; i++)
                {
                    buildingPreview.GetComponent<BuildingComponent>().buildingMeshs[i].material.color = Color.green;
                }
            }
            else
            {
                for (int i = 0; i < buildingPreview.GetComponent<BuildingComponent>().buildingMeshs.Count; i++)
                {
                    buildingPreview.GetComponent<BuildingComponent>().buildingMeshs[i].material.color = Color.red;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < buildingPreview.GetComponent<BuildingComponent>().buildingMeshs.Count; i++)
                {
                    buildingPreview.GetComponent<BuildingComponent>().buildingMeshs[i].material.color = Color.white;
                }
                BoxCollider[] cols = buildingPreview.GetComponentsInChildren<BoxCollider>();
                for (int i = 0; i < cols.Length; i++)
                {
                    cols[i].isTrigger = false;
                }
                
                buildingPreview = null;
                isBuilding = false;

            }
            if (Input.mouseScrollDelta.y > 0 )
            {
                buildingPreview.transform.RotateAround(buildingPreview.transform.position, Vector3.up, 90);
            }
            if (Input.mouseScrollDelta.y < 0 )
            {
                buildingPreview.transform.RotateAround(buildingPreview.transform.position, Vector3.up, -90);
            }

        }
    }
    // Trois prochaines méthodes a changer quand nb de buildings désignés
    public void BuildTBuilding()
    {
        isBuilding = true;
        currentSelectedBuilding = TbuildingPrefab;
    }
    public void Build3x1Building()
    {
        isBuilding = true;
        currentSelectedBuilding = building3x1Prefab;
    }
    public void Build2x2Building()
    {
        isBuilding = true;
        currentSelectedBuilding = building2x2Prefab;
    }
    
    
}
