using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    LevelData levelData;
    TilemapGroup factoryTilemap;
    TilemapGroup battleTilemap;
    enum MapGroup { Factory, Battle, Null = -1 };
    class TilemapGroup
    {
        public Tilemap height;
        public Tilemap terrain;
        public Tilemap specialTiles;

        public TilemapGroup(Tilemap _height, Tilemap _terrain, Tilemap _specialTiles)
        {
            height = _height;
            terrain = _terrain;
            specialTiles = _specialTiles;
        }

        public void Clear()
        {
            height.ClearAllTiles();
            terrain.ClearAllTiles();
            specialTiles.ClearAllTiles();
        }
    }

    public static int tileSize = 4;
    [Range(0, 100)] public static int variance;

    bool showClearSection;
    bool showGenerateSection = true;

    static bool genFactory;
    static bool genBattle;
    bool clearFactory;
    bool clearBattle;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        levelData = (LevelData)target;

        Init();

        GUILayout.Space(14);

        EditorGUILayout.BeginVertical();
        tileSize = EditorGUILayout.IntField("Tile Size", tileSize);
        variance = EditorGUILayout.IntSlider("Tile Variance", variance, 0, 100);
        EditorGUILayout.EndVertical();

        GUILayout.Space(14);

        showGenerateSection = EditorGUILayout.BeginFoldoutHeaderGroup(showGenerateSection, "Terrain Generation");
        if (showGenerateSection)
        {
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            genFactory = EditorGUILayout.Toggle("Factory Map", genFactory);
            genBattle = EditorGUILayout.Toggle("Battle Map", genBattle);
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Generate"))
            {
                if (genBattle || genFactory)
                {
                    ResizeTilemap();

                    if (genFactory)
                    {
                        //Debug.Log("origin = " + factoryTilemap.height.origin);
                        List<Tile> grid = GenerateGrid(factoryTilemap);
                        levelData.serializableGrid = grid;
                        levelData.LoadGrid();
                        TerrainToggle(MapGroup.Factory, true);
                        Generate3DTerrain(grid, factoryTilemap.height.origin, MapGroup.Factory);
                    }

                    if (genBattle)
                    {
                        List<Tile> grid = GenerateGrid(battleTilemap);
                        TerrainToggle(MapGroup.Battle, true);
                        Generate3DTerrain(grid, battleTilemap.height.origin, MapGroup.Battle);

                    }

                }
                else
                {
                    if (EditorUtility.DisplayDialog("Message", "No Map Selected", "Close")) { }
                }

            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear 3D"))
            {
                if (genBattle || genFactory)
                {
                    if (genFactory)
                        ClearTerrain(GetContainer(MapGroup.Factory));

                    if (genBattle)
                        ClearTerrain(GetContainer(MapGroup.Battle));

                }
                else
                {
                    if (EditorUtility.DisplayDialog("Message", "No Map Selected", "Close")) { }
                }

            }

            GUI.backgroundColor = Color.cyan;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(8);
            if (GUILayout.Button("Toggle 3D"))
            {
                TerrainToggle();
            }

            GUILayout.Space(15);
            GUI.backgroundColor = Color.white;

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        showClearSection = EditorGUILayout.BeginFoldoutHeaderGroup(showClearSection, "Clear Tilemap Data");
        if (showClearSection)
        {
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            clearFactory = EditorGUILayout.Toggle("Clear Factory Map", clearFactory);
            clearBattle = EditorGUILayout.Toggle("Clear Battle Map", clearBattle);
            EditorGUILayout.EndVertical();

            GUILayout.Space(8);

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear"))
            {
                if (clearBattle || clearFactory)
                {
                    if (EditorUtility.DisplayDialog("Warning !", "Tilemap will be cleared. This action is irreversible, proceed anyway ?", "YES", "NO"))
                    {
                        if (clearFactory)
                        {
                            factoryTilemap.Clear();
                            ClearTerrain(GetContainer(MapGroup.Factory));
                        }
                            

                        if (clearBattle)
                        {
                            battleTilemap.Clear();
                            ClearTerrain(GetContainer(MapGroup.Battle));
                        }
                            
                    }
                }
                else
                {
                    if (EditorUtility.DisplayDialog("Message", "No Tilemap selected", "Close")) { }
                }




            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();


        }
        EditorGUILayout.EndFoldoutHeaderGroup();

    }


    #region Methods
    void Init()
    {
        if (levelData == null)
            return;

        if (factoryTilemap != null && battleTilemap != null)
            return;

        Transform factoryTilemaps = levelData.transform.Find("Level Painter/FactoryMap");
        Transform battleTilemaps = levelData.transform.Find("Level Painter/BattleMap");

        factoryTilemap = new TilemapGroup(
            factoryTilemaps.Find("Factory_HeightMap").GetComponent<Tilemap>(),
            factoryTilemaps.Find("Factory_Terrain").GetComponent<Tilemap>(),
            factoryTilemaps.Find("Factory_SpecialTiles").GetComponent<Tilemap>());

        battleTilemap = new TilemapGroup(
            battleTilemaps.Find("Battle_HeightMap").GetComponent<Tilemap>(),
            battleTilemaps.Find("Battle_Terrain").GetComponent<Tilemap>(),
            battleTilemaps.Find("Battle_SpecialTiles").GetComponent<Tilemap>());
    }

    void ResizeTilemap()
    {
        factoryTilemap.height.CompressBounds();
        factoryTilemap.terrain.size = factoryTilemap.height.size;
        factoryTilemap.terrain.origin = factoryTilemap.height.origin;
        factoryTilemap.specialTiles.size = factoryTilemap.height.size;
        factoryTilemap.specialTiles.origin = factoryTilemap.height.origin;

        battleTilemap.height.CompressBounds();
        battleTilemap.terrain.size = factoryTilemap.height.size;
        battleTilemap.terrain.origin = factoryTilemap.height.origin;
        battleTilemap.specialTiles.size = factoryTilemap.height.size;
        battleTilemap.specialTiles.origin = factoryTilemap.height.origin;
    }

    List<Tile> GenerateGrid(TilemapGroup _tilemap)
    {
        List<Tile> tempGrid = new List<Tile>();
        BoundsInt bounds = _tilemap.height.cellBounds;
        
        for (int i = bounds.xMin; i < bounds.xMax; i++)
        {
            for (int j = bounds.yMin; j < bounds.yMax; j++)
            {
                Vector3Int p = new Vector3Int(i, j, 0);

                if (factoryTilemap.height.GetTile(p) && factoryTilemap.terrain.GetTile(p))
                {
                    string sh = factoryTilemap.height.GetTile(p).name;
                    string st = factoryTilemap.terrain.GetTile(p).name;
                    string sb = new string("");

                    if (factoryTilemap.specialTiles.GetTile(p))
                        sb = factoryTilemap.specialTiles.GetTile(p).name;

                    //Tile t = new Tile(new Vector2Int(i, j), GetType(st), GetHeight(sh), sb == "TileBlocker");
                    //t.

                    tempGrid.Add(new Tile(new Vector2Int(i, j), GetType(st), GetHeight(sh), sb == "TileBlocker"));
                }
                else tempGrid.Add(new Tile(new Vector2Int(i, j), Tile.Tile_Type.Void));
            }
        }
        return tempGrid;
    }

    int GetHeight(string _tileName)
    {
        int height;
        switch (_tileName)
        {
            case "Height1":
                height = 1;
                break;
            case "Height2":
                height = 2;
                break;
            case "Height3":
                height = 3;
                break;
            case "Height4":
                height = 4;
                break;
            default:
                height = 0;
                break;
        }
        return height;
    }

    Tile.Tile_Type GetType(string _tileName)
    {
        Tile.Tile_Type t;
        switch (_tileName)
        {
            case "Grass":
                t = Tile.Tile_Type.Grass;
                break;
            case "Water":
                t = Tile.Tile_Type.Water;
                break;
            default:
                t = Tile.Tile_Type.Void;
                break;
        }
        return t;
    }

    Transform GetContainer(MapGroup _mapGroup)
    {
        Transform t;

        switch (_mapGroup)
        {
            case MapGroup.Factory:
                t = levelData.transform.Find("FactoryMapTerrain");
                break;
            case MapGroup.Battle:
                t = levelData.transform.Find("BattleMapTerrain");
                break;
            default:
                t = null;
                break;
        }
        return t;
    }

    Vector3 GetOrigin(MapGroup _mapGroup)
    {
        Vector3 v;
        Vector3 temp;

        switch (_mapGroup)
        {
            case MapGroup.Factory:
                temp = factoryTilemap.height.origin;
                v = new Vector3(temp.x, temp.z, temp.y);
                break;
            case MapGroup.Battle:
                temp = battleTilemap.height.origin;
                v = new Vector3(temp.x, temp.z, temp.y);
                break;
            default:
                v = Vector3.zero;
                break;
        }
        return v;
    }

    void ClearTerrain(Transform _container)
    {
        for (int i = _container.childCount - 1; i >= 0; --i)
        {
            if (Application.isEditor)
                DestroyImmediate(_container.GetChild(i).gameObject);
            else Destroy(_container.GetChild(i).gameObject);
        }
    }

    void PlaceVariants(GameObject _instance, int _height, int _vIndex)
    {
        GameObject model = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tiles/Ground/Variants/Rocks_h" + _height.ToString() + ".prefab", typeof(GameObject));
        GameObject variant = PrefabUtility.InstantiatePrefab(model, _instance.transform.Find("Model")) as GameObject;

        MeshRenderer[] mr = variant.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in mr)
            renderer.enabled = false;
        mr[_vIndex].enabled = true;
    }

    void Generate3DTerrain(List<Tile> _grid, Vector3Int _origin, MapGroup _mapGroup)
    {
        Dictionary<Vector2, Tile> tempGrid = new Dictionary<Vector2, Tile>();
        foreach (Tile tile in _grid)
            tempGrid[tile.gridPos] = tile;

        Transform container = GetContainer(_mapGroup);
        if (container.childCount > 0)
            ClearTerrain(container);

        Vector3 origin = GetOrigin(_mapGroup);

        foreach (Tile _tile in _grid)
        {
            AdjacentTiles adj = GridUtilities.GetAdjacentTiles(tempGrid,_tile.gridPos);

            GameObject instance = null;
            GameObject asset = null;

            int direction = Random.Range(0, 4);
            float varianceCheck = Random.Range(0f, 1f);
            int variantIndex = Random.Range(0, 3);

            bool waterfallBlock = false;

            switch (_tile.type)
            {
                case Tile.Tile_Type.Void:
                    break;


                case Tile.Tile_Type.Grass:

                    asset = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tiles/Ground/Grass_h" + _tile.height.ToString() + ".prefab", typeof (GameObject));
                    instance = PrefabUtility.InstantiatePrefab(asset, container) as GameObject;

                    bool b = false;

                    instance.transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);
                    foreach (Tile t in adj)
                        if (t == null || t.height < _tile.height)
                            b = true;
                    if (b && varianceCheck <= (float)variance / 100)
                        PlaceVariants(instance, _tile.height, variantIndex);
                    break;




                case Tile.Tile_Type.Water:

                    asset = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tiles/Water/Water_h" + _tile.height.ToString() + ".prefab", typeof(GameObject));
                    instance = PrefabUtility.InstantiatePrefab(asset, container) as GameObject;

                    Transform baseTile = instance.transform.Find("Model");
                    Transform water = instance.transform.Find("Water");

                    baseTile.rotation = Quaternion.Euler(0f, 90f * direction, 0f);

                    foreach (Tile tAdj in adj)
                    {
                        if (tAdj != null && tAdj.type == Tile.Tile_Type.Water)
                        {
                            Vector2 pointer = tAdj.gridPos - _tile.gridPos;

                            if (tAdj.height < _tile.height)
                            {
                                Transform edges = water.Find("Edges");
                                Transform _t = null;
                                List<GameObject> w_tilesetList = null;
                                int i = GridUtilities.GetDirectionIndex(pointer);

                                // Directions are numbered clockwise from 0 to 7 starting with up = 0
                                // If i is Odd => Tiles in Diagonal (UpRight = 1, DownRight = 3, DownLeft = 5, UpLeft = 7)

                                if (i % 2 != 0)
                                {
                                    // If Conditions to detect Out Corner :
                                    // 
                                    if ((GridUtilities.GetAdjacentFromIndex(adj, i - 1).height < _tile.height ||
                                        (GridUtilities.GetAdjacentFromIndex(adj, i - 1).type == Tile.Tile_Type.Grass &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i - 1).height == _tile.height))
                                        &&
                                        (GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height < _tile.height ||
                                        (GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).type == Tile.Tile_Type.Grass &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height == _tile.height)))
                                    {
                                        //Do
                                        _t = edges.GetChild(i);
                                        w_tilesetList = new List<GameObject>();
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_CornerOut_Top.prefab", typeof(GameObject)));
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_CornerOut_Bottom.prefab", typeof(GameObject)));
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_CornerOut_Body.prefab", typeof(GameObject)));
                                    }
                                    // If Conditions to detect In Corner
                                    else if (GridUtilities.GetAdjacentFromIndex(adj, i - 1).height == _tile.height &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i - 1).type == Tile.Tile_Type.Water &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height == _tile.height &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).type == Tile.Tile_Type.Water)
                                    {
                                        //Do
                                        _t = edges.GetChild(i);
                                        w_tilesetList = new List<GameObject>();
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_CornerIn_Top.prefab", typeof(GameObject)));
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_CornerIn_Bottom.prefab", typeof(GameObject)));
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_CornerIn_Body.prefab", typeof(GameObject)));
                                    }

                                }
                                else // If i is Even => Tiles in Cardinal (Up = 0, Right = 2, Down = 4, Left = 6)
                                {
                                    if (GridUtilities.GetAdjacentFromIndex(adj, i == 0 ? 7 : i - 1).height >= _tile.height && 
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 0 ? 7 : i - 1).type == Tile.Tile_Type.Water 
                                        ||
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).height == _tile.height &&
                                        GridUtilities.GetAdjacentFromIndex(adj, i == 7 ? 0 : i + 1).type == Tile.Tile_Type.Water)
                                    {
                                        
                                        _t = null;
                                    }
                                        

                                    else if (GridUtilities.GetAdjacentFromIndex(adj, i).height >= _tile.height)
                                    {
                                        _t = null;
                                    }

                                    else
                                    {
                                        _t = edges.GetChild(i);
                                        w_tilesetList = new List<GameObject>();
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_Straight_Top.prefab", typeof(GameObject)));
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_Straight_Bottom.prefab", typeof(GameObject)));
                                        w_tilesetList.Add((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileParts/Water/Waterfall_Straight_Body.prefab", typeof(GameObject)));
                                    }
                                }

                                if (_t != null && w_tilesetList != null)
                                {
                                    //GridUtilities.GetAdjacentFromIndex(adj, i).isBlocked = true;
                                    //tempGrid[GridUtilities.GetAdjacentFromIndex(adj, i).gridPos].isBlocked = true;
                                    
                                    waterfallBlock = true;
                                    int h = _tile.height - tAdj.height;
                                    for (int j = 0; j < h + 1; j++)
                                    {
                                        GameObject _instance = PrefabUtility.InstantiatePrefab(w_tilesetList[j > 2 ? 2 : j], _t) as GameObject;
                                        _instance.transform.rotation = Quaternion.Euler(0f, 90f * (i / 2), 0f);
                                        _instance.transform.position += Vector3.down * tileSize * (j == 0 ? 0 : (j >= 2 ? j - 1 : h));
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
                Vector3 wordlPos = new Vector3((_tile.gridPos.x + 0.5f) * tileSize, 0, (_tile.gridPos.y +0.5f) * tileSize);
                instance.transform.position = wordlPos;

                instance.GetComponent<TileComponent>().gridPos = _tile.gridPos;
                instance.GetComponent<TileGizmo>().hasBlocker = _tile.isBlocked && !waterfallBlock;

                _tile.associatedGO = instance;
                if (_tile.type == Tile.Tile_Type.Water && waterfallBlock)
                    _tile.isBlocked = true;
            }
        }

        if(_mapGroup == MapGroup.Factory)
        {
            levelData.grid = tempGrid;
            levelData.SaveGrid();
        }
    }

    void TerrainToggle(MapGroup _mapGroup = MapGroup.Null ,bool forceActive = false)
    {
        if (_mapGroup != MapGroup.Null)
        {
            Transform container = GetContainer(_mapGroup);
            bool x = container.gameObject.activeSelf;
            container.gameObject.SetActive(forceActive ? true : !x);
        }
        else
        {
            Transform containerA = GetContainer(MapGroup.Factory);
            Transform containerB = GetContainer(MapGroup.Battle);
            bool a = containerA.gameObject.activeSelf;
            bool b = containerB.gameObject.activeSelf;

            containerA.gameObject.SetActive(forceActive ? true : !a);
            containerB.gameObject.SetActive(forceActive ? true : !b);
        }
    }

    #endregion

}