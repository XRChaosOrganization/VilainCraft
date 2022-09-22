using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileComponent : MonoBehaviour /*, IPointerEnterHandler , IPointerExitHandler*/  
{
    public Vector2 gridPos;
    public SpriteRenderer tile_gridCell_sr;

    private void Awake()
    {
        tile_gridCell_sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        GameEvents.current.onDisplayGrid += DisplayGrid;
    }

    private void OnDestroy()
    {
        GameEvents.current.onDisplayGrid -= DisplayGrid;
    }

    public void DisplayGrid(bool b)
    {
        tile_gridCell_sr.enabled = b;
    }


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
