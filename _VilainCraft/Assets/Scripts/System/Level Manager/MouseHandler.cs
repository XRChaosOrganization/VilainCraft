using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseHandler : MonoBehaviour
{
    public static MouseHandler current;

    [System.Serializable]
    public class Tile_MO
    {
        public TileComponent tileComponent;
        public Vector3 worldPos;
        public Vector3 tileAnchor;
        public Tile tile;
        public Vector2 gridPos;
        public SpriteRenderer gridCell_sr;
        

        public Tile_MO(RaycastHit hit)
        {
            tileComponent = hit.collider.GetComponentInParent<TileComponent>();
            gridCell_sr = tileComponent.tile_gridCell_sr;
            worldPos = tileComponent.transform.position;
            LevelData.current.grid.TryGetValue(tileComponent.gridPos, out tile);
            tileAnchor = worldPos + Vector3.up * tile.height * 4;
            gridPos = tileComponent.gridPos;
        }
    }

    
    [HideInInspector] public Vector3 screenPoint;

    Plane voidPlane;
    [HideInInspector] public Vector3 voidPos;

    [Space(15)]
    public LayerMask tileLayerMask;
    [HideInInspector]public Tile_MO tile_mo;
    Collider tileCol;

    //Ajouter un Header pour les Buildings

    private void Awake()
    {
        current = this;
        voidPlane = new Plane(Vector3.up, -4f);
    }

    private void LateUpdate()
    {
        MouseToScreenPoint();
        TileMouseOver();
        VoidMouseOver();
    }


    #region Methods

    private void VoidMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float enter;
        if (voidPlane.Raycast(ray, out enter))
            voidPos = ray.GetPoint(enter);

    }

    void TileMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, tileLayerMask))
        {

            if (hit.collider != tileCol)
            {
                tile_mo = new Tile_MO(hit);
                tileCol = hit.collider;
            }
        }
        else
        {
            tile_mo = null;
            tileCol = null;
        }
    }

    void MouseToScreenPoint()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 10f;
        screenPoint = Camera.main.ScreenToWorldPoint(pos);
    }

    //Ajouter une Methode pour le Mouseover des Buildings


    #endregion

}
