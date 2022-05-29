using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public bool isDemolishMode;
    public List<BuildingSensor> colliders;
    public Transform buildingContainer;
    

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
            
            if (Input.GetMouseButtonDown(0) && _currentBC.buildable == true)
            {
                BuildPreview(_currentBC);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Cancel();
            }
            if (Input.mouseScrollDelta.y > 0 )
            {
                RotatePreview(buildingPreview.transform,90);
            }
            if (Input.mouseScrollDelta.y < 0 )
            {
                RotatePreview(buildingPreview.transform,-90);
            }
        }
    }
    public void PreviewDisplay(BuildingComponent _building)
    {
        if (_building.buildable == true)
        {
            for (int i = 0; i < _building.buildingMeshs.Count; i++)
            {
                _building.buildingMeshs[i].material.color = Color.green;
            }
        }
        else
        {
            for (int i = 0; i < _building.buildingMeshs.Count; i++)
            {
                _building.buildingMeshs[i].material.color = Color.red;
            }
        }
    }
    void BuildPreview(BuildingComponent _currentBC)
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
        BoxCollider[] cols;
        cols = buildingPreview.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].isTrigger = false;
        }
        BuildingSensor[] sensors;
        sensors = buildingPreview.GetComponentsInChildren<BuildingSensor>();
        for (int i = 0; i < sensors.Length; i++)
        {
            colliders.Add(sensors[i]);
        }
        buildingPreview = null;
    }
    void Cancel()
    {
        Destroy(buildingPreview);
        isBuilding = false;
        isDemolishMode = false;
    }
    void RotatePreview(Transform _preview , int _angle)
    {
        _preview.RotateAround(_preview.position, Vector3.up, _angle);
    }
    
    // Trois prochaines méthodes a changer quand nb de buildings désignés
    public void BuildTBuilding()
    {
        isDemolishMode = false;
        isBuilding = true;
        currentSelectedBuilding = TbuildingPrefab;
    }
    public void Build3x1Building()
    {
        isDemolishMode = false;
        isBuilding = true;
        currentSelectedBuilding = building3x1Prefab;
    }
    public void Build2x2Building()
    {
        isDemolishMode = false;
        isBuilding = true;
        currentSelectedBuilding = building2x2Prefab;
    }
    public void DemolishMode()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].gameObject.layer = 7;
        }
        Destroy(buildingPreview);
        isBuilding = false;
        isDemolishMode = true;

    }
    
}
