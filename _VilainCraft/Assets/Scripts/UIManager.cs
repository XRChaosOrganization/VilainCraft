using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Camera cam;
    private GameObject currentBuilding;
    public GameObject buildingPrefab;
    public static UIManager current;
    public bool isBuilding =true;

    private void Awake()
    {
        cam = Camera.main;
        current = this;
    }
    private void Update()
    {
      // (isBuilding)
      //
      //  Vector3 mouseDir =  cam.transform.position -cam.ScreenToWorldPoint(Input.mousePosition) ;
      //  RaycastHit hit;
      //  Debug.DrawRay(cam.transform.position, mouseDir,Color.magenta);
      //  if (Physics.Raycast(cam.transform.position, mouseDir, out hit))
      //  {
      //      Debug.Log(hit.transform);
      //      if (hit.collider.gameObject.GetComponent<TileComponent>().hasBuilding == false)
      //      {
      //          Debug.Log("Cette tile n'a pas de building");
      //          GameObject building = (GameObject)Instantiate(buildingPrefab, hit.collider.gameObject.transform.position,Quaternion.identity);
      //          currentBuilding = building;
      //
      //      }
      //      else
      //      {
      //          Debug.Log("Cette tile à un building");
      //      }
      //  }
      //
      //  currentBuilding.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            
      //}
    
    }
    public void Build()
    {
        isBuilding = true;
        
    }
    public void BuildingPlacement()
    {

    }
}
