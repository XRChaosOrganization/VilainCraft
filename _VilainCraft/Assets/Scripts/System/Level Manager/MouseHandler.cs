using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    [System.Serializable]
    public class Tile_MO
    {
        public TileComponent tileComponent;
        public Vector3 worldPos;
        public Vector3 tileAnchor;
        public Tile tile;
        public SpriteRenderer tile_gridCell_sr;

        public Tile_MO(RaycastHit hit)
        {
            tileComponent = hit.collider.GetComponentInParent<TileComponent>();
            tile_gridCell_sr = tileComponent.tile_gridCell_sr;
            worldPos = tileComponent.transform.position;
            GridManager.current.grid.TryGetValue(tileComponent.gridPos, out tile);
            tileAnchor = worldPos + Vector3.up * tile.height * 4;
        }
    }

    [Header("Tile")]    
    public LayerMask tileLayerMask;
    public Tile_MO tile_mo;
    Collider tileCol;

    //Ajouter un Header pour les Buildings

    private void Update()
    {
        TileMouseOver();
    }


    #region Methods

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

    //Ajouter une Methode pour le Mouseover des Buildings


    #endregion

}
