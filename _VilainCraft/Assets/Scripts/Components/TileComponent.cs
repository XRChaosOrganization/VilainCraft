using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileComponent : MonoBehaviour /*, IPointerEnterHandler , IPointerExitHandler*/  
{
    public Vector2 gridPos;
    [HideInInspector] public SpriteRenderer tile_gridCell_sr;



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






}
