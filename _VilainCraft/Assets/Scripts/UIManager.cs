using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Camera cam;
    public GameObject TbuildingPrefab;
    public GameObject building3x1Prefab;
    public GameObject building2x2Prefab;
    public static UIManager current;
    public bool isBuilding =true;

    private void Awake()
    {
        cam = Camera.main;
        current = this;
    }
    private void Update()
    {
    
    }
    public void BuildTBuilding()
    {
        isBuilding = true;
        
        for (int i = 0; i < GameManager.gm.tilesList.Count; i++)
        {
            GameManager.gm.tilesList[i].buildingPrefab = TbuildingPrefab;
        }
        
    }
    public void Build3x1Building()
    {
        isBuilding = true;
        
        for (int i = 0; i < GameManager.gm.tilesList.Count; i++)
        {
            GameManager.gm.tilesList[i].buildingPrefab = building3x1Prefab;
        }
        
    }
    public void Build2x2Building()
    {
        isBuilding = true;
        
        for (int i = 0; i < GameManager.gm.tilesList.Count; i++)
        {
            GameManager.gm.tilesList[i].buildingPrefab = building2x2Prefab;
        }
        
    }
    
}
