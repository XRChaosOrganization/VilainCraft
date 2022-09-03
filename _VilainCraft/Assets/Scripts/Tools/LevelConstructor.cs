using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[System.Serializable]
public class MapData
{
    public Sprite heightMap;
    public Sprite waterMap;

}


public class LevelConstructor : MonoBehaviour
{
    public int tileSize;

    public Tileset tileset;
    public Transform tileContainer;
    public MapData mapData;

    GridManager gm;

    [Range(0, 100)] public int variance;


    #region Public Methods

    public void ReadMapData()
    {
        Texture2D texture = mapData.heightMap.texture;
        Rect heightRect = mapData.heightMap.rect;
        Rect waterRect = mapData.waterMap.rect;
        IDictionary<Vector2, Tile> data = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < heightRect.width; x++)
        {
            for (int y = 0; y < heightRect.height; y++)
            {
                Tile tile;
                Vector2 gridPos = new Vector2(x, y);
                Color color = texture.GetPixel(x + (int)heightRect.x, y + (int)heightRect.y);
                int step = 256 / (tileset.ground.Count + 1);
                int height = (int)(color.r * 256 / step);

                if (texture.GetPixel(x + (int)waterRect.x, y + (int)waterRect.y).b >= 0.3)
                    tile = new Tile_Water(gridPos, height);
                else if (height == 0)
                    tile = new Tile_Void(gridPos);
                else
                    tile = new Tile_Ground(gridPos, height);
                    
                data.Add(gridPos, tile);

            }
        }
        gm.grid = new Dictionary<Vector2, Tile>(data);
        gm.SaveGrid();
        
    }

    public void PlaceTiles()
    {
        if (tileset.ground.Count == 0)
        {
            Debug.LogError(tileset.name + ".terrain is empty !");
            return;
        }

        foreach (KeyValuePair<Vector2,Tile> pair in gm.grid)
        {
            int height = pair.Value.height;
            Vector2 gridPos = pair.Key;
            AdjacentTiles adj = GridUtilities.GetAdjacentTiles(gm.grid, gridPos);

            int direction = Random.Range(0, 4);
            float varianceCheck = Random.Range(0f, 1f);
            int variantIndex = Random.Range(0, 3);

            GameObject instance = null;
            
            switch (pair.Value.type)
            {
                case Tile.Tile_Type.Void:
                    break;



                case Tile.Tile_Type.Ground:
                    instance = PrefabUtility.InstantiatePrefab(tileset.ground[height - 1], tileContainer) as GameObject;
                    Tile_Ground tile = (Tile_Ground)pair.Value;
                    bool b = false;

                    instance.transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);
                    foreach (Tile _tile in adj)
                        if (_tile == null || _tile.height < tile.height)
                            b = true;
                    if (b && varianceCheck <= (float)variance / 100)
                        PlaceVariants(instance, height, variantIndex);
                    break;




                case Tile.Tile_Type.Water:
                    instance = PrefabUtility.InstantiatePrefab(tileset.water[height - 1], tileContainer) as GameObject;
                    Transform baseTile = instance.transform.Find("Model");
                    Transform water = instance.transform.Find("Water");
                    baseTile.rotation = Quaternion.Euler(0f, 90f * direction, 0f);

                    foreach (Tile _tile in adj)
                    {
                        Vector2 pointer = _tile.gridPos - pair.Key;

                        if (_tile != null && _tile.type == Tile.Tile_Type.Water)
                        {
                            if (_tile.height < pair.Value.height)
                            {
                                Transform edges = water.Find("Edges");
                                Transform _t = null;
                                List<GameObject> w_tilesetList = null;
                                int i = GridUtilities.GetDirectionIndex(pointer);



                                // Directions are numbered clockwise from 0 to 7 starting with up = 0
                                // i is ODD => Tiles in Diagonal (UpRight = 1, DownRight = 3, DownLeft = 5, UpLeft = 7)

                                if (i % 2 != 0)
                                {
                                    // Conditions to detect Out Corner
                                    if ((GridUtilities.GetAdjacentFromIndex(adj, i - 1).height < pair.Value.height ||
                                        (GridUtilities.GetAdjacentFromIndex(adj, i - 1).type == Tile.Tile_Type.Ground &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i - 1).height == pair.Value.height))
                                        &&
                                        (GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height < pair.Value.height ||
                                        (GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).type == Tile.Tile_Type.Ground &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height == pair.Value.height)))
                                    {
                                        
                                        _t = edges.GetChild(i);
                                        w_tilesetList = tileset.waterfall.cornerOut;
                                    }
                                    // Conditions to detect In Corner
                                    else if (GridUtilities.GetAdjacentFromIndex(adj, i - 1).height == pair.Value.height &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height == pair.Value.height)
                                    { 
                                        _t = edges.GetChild(i);
                                        w_tilesetList = tileset.waterfall.cornerIn;
                                    }

                                }
                                else // i is Even => Tiles in Cardinal (Up = 0, Right = 2, Down = 4, Left = 6)
                                {
                                    if (GridUtilities.GetAdjacentFromIndex(adj, i == 0 ? 7 : i - 1).height == pair.Value.height ||
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height == pair.Value.height)
                                        _t = null;
                                    else 
                                    {
                                        _t = edges.GetChild(i);
                                        w_tilesetList = tileset.waterfall.straight;
                                    }
                                    
                                }


                                if (_t != null && w_tilesetList != null)
                                {
                                    int h = pair.Value.height - _tile.height;
                                    for (int j = 0; j < h + 1; j++)
                                    {
                                        GameObject _instance = PrefabUtility.InstantiatePrefab(w_tilesetList[j > 2 ? 2 : j], _t) as GameObject;
                                        _instance.transform.rotation = Quaternion.Euler(0f, 90f * (i / 2), 0f);
                                        _instance.transform.position += Vector3.down * tileSize * (j == 0 ? 0: (j >= 2 ? j-1 : h));
                                    }
                                }          
                            }
                        }
                    }

                    break;

                default:
                    break;
            }

            if (instance != null)
            {
                instance.transform.position = new Vector3(gridPos.x * tileSize, 0, gridPos.y * tileSize);
                instance.GetComponent<TileComponent>().gridPos = gridPos;
                pair.Value.associatedGO = instance;
            }
                
            
        }
        
    }



    #endregion


    #region Map Generation

    public void GenerateTerrain()
    {

        if (tileContainer.childCount > 0)
            ClearTerrain();

        gm = FindObjectOfType<GridManager>();

        ReadMapData();
        PlaceTiles();

    }

    public void ClearTerrain()
    {
        Debug.ClearDeveloperConsole();
        for (int i = tileContainer.childCount - 1; i >= 0; --i)
        {
            if (Application.isEditor)
                DestroyImmediate(tileContainer.GetChild(i).gameObject);
            else Destroy(tileContainer.GetChild(i).gameObject);

        }
    }

    void PlaceVariants(GameObject _instance, int _height, int _vIndex)
    {
        GameObject variant = PrefabUtility.InstantiatePrefab(tileset.groundVariants[_height - 1], _instance.transform.Find("Model")) as GameObject;

        MeshRenderer[] mr = variant.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in mr)
            renderer.enabled = false;
        mr[_vIndex].enabled = true;
    }

    #endregion
}

#region Custom Editor

[CustomEditor(typeof(LevelConstructor))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelConstructor constructor = (LevelConstructor)target;
       

        GUILayout.Space(14);
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generate Terrain"))
            constructor.GenerateTerrain();

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Terrain"))
            constructor.ClearTerrain();

    }
}

#endregion
