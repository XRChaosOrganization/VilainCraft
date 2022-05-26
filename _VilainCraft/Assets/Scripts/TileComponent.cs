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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIManager.current.isBuilding)
        {
            currentBuilding = buildingPreview;
            buildingPreview.GetComponent<MeshRenderer>().material.color = Color.white;
            hasBuilding = true;
            buildingPreview = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIManager.current.isBuilding && buildingPreview == null)
        {
            buildingPreview = (GameObject)Instantiate(buildingPrefab, transform.position, Quaternion.identity);
            buildingPreview.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.current.isBuilding)
        {
            Destroy(buildingPreview);
            
        }
    }
    
    

    
}
