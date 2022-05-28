using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileComponent : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler  
{
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIManager.current.isBuilding)
        {
            UIManager.current.currentSelectedTile = transform;
            if (UIManager.current.buildingPreview == null)
            {
                UIManager.current.buildingPreview = (GameObject)Instantiate(UIManager.current.currentSelectedBuilding, transform.position, Quaternion.identity);
            }
            else
            {
                UIManager.current.buildingPreview.transform.position = transform.position;
            }
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.current.currentSelectedTile = null;
    }
}
