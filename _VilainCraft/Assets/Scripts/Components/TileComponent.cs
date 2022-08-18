using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileComponent : MonoBehaviour /*, IPointerEnterHandler , IPointerExitHandler*/  
{
    public Vector2 gridPos;
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (UIManager.current.isBuilding)
    //    {
    //        UIManager.current.currentSelectedTile = transform;
    //        if (UIManager.current.buildingPreview == null)
    //        {
    //            UIManager.current.buildingPreview = (GameObject)Instantiate(UIManager.current.currentSelectedBuilding, transform.position, Quaternion.identity,UIManager.current.buildingContainer);
    //        }
    //        else
    //        {
    //            UIManager.current.buildingPreview.transform.position = transform.position;
    //        }
    //        UIManager.current.PreviewDisplay(UIManager.current.buildingPreview.GetComponent<BuildingComponent>());
    //    }
    //}
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    UIManager.current.currentSelectedTile = null;
    //}
}
