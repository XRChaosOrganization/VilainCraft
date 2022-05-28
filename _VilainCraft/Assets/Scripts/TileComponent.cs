using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileComponent : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler , IPointerClickHandler 
{
    public bool hasBuilding ;
    GameObject currentBuilding;
    public GameObject buildingPreview;
    public GameObject buildingPrefab;
    bool onPreview;

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0 && onPreview)
        {
            buildingPreview.transform.RotateAround(buildingPreview.transform.position, Vector3.up, 90);
        }
        if (Input.mouseScrollDelta.y < 0 && onPreview)
        {
            buildingPreview.transform.RotateAround(buildingPreview.transform.position, Vector3.up, -90);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIManager.current.isBuilding)
        {
            currentBuilding = buildingPreview;
            for (int i = 0; i < buildingPreview.GetComponent<BuildingComponent>().buildingMeshs.Count; i++)
            {
                buildingPreview.GetComponent<BuildingComponent>().buildingMeshs[i].material.color = Color.white;
            }
            hasBuilding = true;
            onPreview = false;
            buildingPreview = null;
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIManager.current.isBuilding && buildingPreview == null)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            onPreview = true;
            buildingPreview = (GameObject)Instantiate(buildingPrefab, transform.position, Quaternion.identity);
            for (int i = 0; i < buildingPreview.GetComponent<BuildingComponent>().buildingMeshs.Count; i++)
            {
                buildingPreview.GetComponent<BuildingComponent>().buildingMeshs[i].material.color = Color.green;
            }
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.current.isBuilding)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.clear;
            onPreview = false;
            Destroy(buildingPreview);
            
        }
    }
    
    

    
}
