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

        if (isBuilding && buildingPreview!= null && currentSelectedTile != null)
        {
            BuildingComponent _currentBC = buildingPreview.GetComponent<BuildingComponent>();
            if (_currentBC.buildable == true)
            {
                for (int i = 0; i < _currentBC.buildingMeshs.Count; i++)
                {
                    _currentBC.buildingMeshs[i].material.color = Color.green;
                }
            }
            else
            {
                for (int i = 0; i < _currentBC.buildingMeshs.Count; i++)
                {
                    _currentBC.buildingMeshs[i].material.color = Color.red;
                }
            }
            if (Input.GetMouseButtonDown(0) && _currentBC.buildable == true)
            {
                for (int i = 0; i < _currentBC.buildingMeshs.Count; i++)
                {
                    _currentBC.buildingMeshs[i].material.color = Color.white;
                }
                Rigidbody[] rbs;
                rbs = buildingPreview.GetComponentsInChildren<Rigidbody>();
                for (int i = 0; i < rbs.Length; i++)
                {
                    rbs[i].constraints = RigidbodyConstraints.FreezePosition;
                }
                buildingPreview = null;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(buildingPreview);
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
