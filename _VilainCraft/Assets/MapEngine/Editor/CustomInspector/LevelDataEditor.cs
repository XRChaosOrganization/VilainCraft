using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using MapEngine;
using Tile = MapEngine.Tile;
using Cinemachine;

namespace MapEngineEditor
{
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
        bool showCamSection = true;

        static bool genFactory;
        static bool genBattle;
        bool clearFactory;
        bool clearBattle;

        LevelCameraController mainCam;
        CinemachineComposer composer;
        float fov;
        Vector3 offset;


        #region GUI
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
                ShowGenSection();
            EditorGUILayout.EndFoldoutHeaderGroup();

            showClearSection = EditorGUILayout.BeginFoldoutHeaderGroup(showClearSection, "Clear Tilemap Data");
            if (showClearSection)
                ShowClearSection();
            EditorGUILayout.EndFoldoutHeaderGroup();

            showCamSection = EditorGUILayout.BeginFoldoutHeaderGroup(showCamSection, "Camera");
            if (showCamSection)
                ShowCamSection();
            EditorGUILayout.EndFoldoutHeaderGroup();




        }

        void ShowGenSection()
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
                        List<Tile> gridF = GenerateGrid(factoryTilemap);
                        levelData.serializableGrid = gridF;
                        levelData.LoadGrid();
                        TerrainToggle(MapGroup.Factory, true);
                        Generate3DTerrain(gridF, MapGroup.Factory);
                        SetCamera(MapGroup.Factory);
                    }

                    if (genBattle)
                    {
                        List<Tile> gridB = GenerateGrid(battleTilemap);
                        TerrainToggle(MapGroup.Battle, true);
                        Generate3DTerrain(gridB, MapGroup.Battle);
                        SetCamera(MapGroup.Battle);
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
        void ShowClearSection()
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
                            levelData.grid = null;
                            levelData.serializableGrid = null;
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

        void ShowCamSection()
        {
            GUILayout.Space(5);

            if (mainCam == null)
                mainCam = FindObjectOfType<LevelCameraController>();
            
            if (composer == null && mainCam.activeCam != null)
                composer = mainCam.activeCam.GetCinemachineComponent<CinemachineComposer>();


            bool hasCam = mainCam;

            if (!hasCam)
            {
                GUILayout.Space(15);
                EditorGUILayout.HelpBox("No Camera containing a CameraController has been found", MessageType.Warning);
                GUILayout.Space(15);
            }

            else
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Factory Cam"))
                {
                    GetCamera(MapGroup.Battle).gameObject.SetActive(false);
                    GetCamera(MapGroup.Factory).gameObject.SetActive(true);
                }

                if (GUILayout.Button("Battle Cam"))
                {
                    GetCamera(MapGroup.Factory).gameObject.SetActive(false);
                    GetCamera(MapGroup.Battle).gameObject.SetActive(true);
                }
                EditorGUILayout.EndHorizontal();

                if(mainCam.activeCam != null)
                {
                    fov = mainCam.activeCam.m_Lens.FieldOfView;
                    offset = composer.m_TrackedObjectOffset;
                }
                

                GUILayout.Space(5);
                EditorGUI.BeginChangeCheck();

                fov = EditorGUILayout.FloatField("FOV", fov);
                offset = EditorGUILayout.Vector3Field("Offset", offset);

                if (EditorGUI.EndChangeCheck())
                {
                    mainCam.activeCam.m_Lens.FieldOfView = fov;
                    mainCam.activeCam.GetComponentInParent<LevelVCamComponent>().farthestFOV = fov;
                    composer.m_TrackedObjectOffset = offset;
                    EditorUtility.SetDirty(target);
                }
            }
        }

        #endregion


        #region Private Methods
        void Init()
        {
            if (levelData == null)
                return;

            if (factoryTilemap != null && battleTilemap != null)
                return;

            Transform factoryTilemaps = levelData.transform.Find("PainterTilemaps/FactoryMap");
            Transform battleTilemaps = levelData.transform.Find("PainterTilemaps/BattleMap");

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
            battleTilemap.terrain.size = battleTilemap.height.size;
            battleTilemap.terrain.origin = battleTilemap.height.origin;
            battleTilemap.specialTiles.size = battleTilemap.height.size;
            battleTilemap.specialTiles.origin = battleTilemap.height.origin;

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

                    if (_tilemap.height.GetTile(p) && _tilemap.terrain.GetTile(p))
                    {
                        string sh = _tilemap.height.GetTile(p).name;
                        string st = _tilemap.terrain.GetTile(p).name;
                        string sx = new string("");

                        if (_tilemap.specialTiles.GetTile(p))
                            sx = _tilemap.specialTiles.GetTile(p).name;

                        Tile t = new Tile(new Vector2Int(i, j), GetType(st), GetHeight(sh), sx == "TileBlocker");
                        t.node = GetNode(sx);

                        tempGrid.Add(t);
                    }
                    else tempGrid.Add(new Tile(new Vector2Int(i, j), Tile.Tile_Type.Void));
                }
            }
            return tempGrid;
        }

        int GetHeight(string _tileName)
        {
            switch (_tileName)
            {
                case "Height1":
                    return 1;
                case "Height2":
                    return 2;
                case "Height3":
                    return 3;
                case "Height4":
                    return 4;
                default:
                    return 0;
            }
        }

        TilemapGroup GetTilemapGroup(MapGroup _mapGroup)
        {
            switch (_mapGroup)
            {
                case MapGroup.Factory:
                    return factoryTilemap;
                case MapGroup.Battle:
                    return battleTilemap;
                default:
                    return null;
            }
        }

        Tile.Tile_Type GetType(string _tileName)
        {
            switch (_tileName)
            {
                case "Grass":
                    return Tile.Tile_Type.Grass;
                case "Water":
                    return Tile.Tile_Type.Water;
                default:
                    return Tile.Tile_Type.Void;
            }
        }

        GameObject GetNode(string _tileName)
        {
            string path = "Assets/Prefabs/Nodes";
            string extension = ".prefab";
            switch (_tileName)
            {
                case "BlankNode":
                    return (GameObject)AssetDatabase.LoadAssetAtPath(path + "BlankNode" + extension, typeof(GameObject));
                default:
                    return null;
            }
        }

        Transform GetContainer(MapGroup _mapGroup)
        {
            switch (_mapGroup)
            {
                case MapGroup.Factory:
                    return levelData.transform.Find("Level Scene/Factory/Terrain");
                case MapGroup.Battle:
                    Transform t = levelData.transform.Find("Level Scene/Battle/Terrain");
                    levelData.transform.Find("Level Scene/Battle").position = new Vector3(t.position.x, battleTilemap.height.transform.position.y, t.position.z);
                    return t;
                default:
                    return null;
            }
        }

        LevelVCamComponent GetCamera(MapGroup _mapGroup)
        {
            switch (_mapGroup)
            {
                case MapGroup.Factory:
                    return levelData.transform.Find("Level Scene/Factory/Camera").GetComponent<LevelVCamComponent>();
                case MapGroup.Battle:
                    return levelData.transform.Find("Level Scene/Battle/Camera").GetComponent<LevelVCamComponent>();
                default:
                    return null;
            }
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

        void Generate3DTerrain(List<Tile> _grid, MapGroup _mapGroup)
        {
            Dictionary<Vector2, Tile> tempGrid = new Dictionary<Vector2, Tile>();
            foreach (Tile tile in _grid)
                tempGrid[tile.gridPos] = tile;

            Transform container = GetContainer(_mapGroup);
            if (container.childCount > 0)
                ClearTerrain(container);

            foreach (Tile _tile in _grid)
            {
                AdjacentTiles adj = GridUtilities.GetAdjacentTiles(tempGrid, _tile.gridPos);

                GameObject instance = null;
                GameObject asset = null;

                int direction = Random.Range(0, 4);
                float varianceCheck = Random.Range(0f, 1f);
                int variantIndex = Random.Range(0, 3);

                switch (_tile.type)
                {
                    case Tile.Tile_Type.Void:
                        break;

                    case Tile.Tile_Type.Grass:

                        #region Grass Tile Placement
                        asset = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tiles/Ground/Grass_h" + _tile.height.ToString() + ".prefab", typeof(GameObject));
                        instance = PrefabUtility.InstantiatePrefab(asset, container) as GameObject;

                        bool b = false;

                        instance.transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);
                        foreach (Tile t in adj)
                            if (t == null || t.height < _tile.height)
                                b = true;
                        if (b && varianceCheck <= (float)variance / 100)
                            PlaceVariants(instance, _tile.height, variantIndex);
                        #endregion

                        break;

                    case Tile.Tile_Type.Water:

                        #region Water Tile Placement

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

                                        tempGrid[GridUtilities.GetAdjacentFromIndex(adj, i).gridPos].isBlocked = true;
                                        tempGrid[GridUtilities.GetAdjacentFromIndex(adj, i).gridPos].waterfallBlock = true;

                                        Debug.Log(GridUtilities.GetAdjacentFromIndex(adj, i).gridPos);

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
                        #endregion

                        break;

                    default:
                        break;
                }


                if (instance != null)
                {
                    Vector3 wordlPos = new Vector3((_tile.gridPos.x + 0.5f) * tileSize, 0, (_tile.gridPos.y + 0.5f) * tileSize);
                    instance.transform.position = wordlPos + Vector3.up * container.position.y;

                    instance.GetComponent<TileComponent>().gridPos = _tile.gridPos;
                    TileGizmo gizmo = instance.GetComponent<TileGizmo>();
                    gizmo.hasBlocker = tempGrid[_tile.gridPos].isBlocked && !tempGrid[_tile.gridPos].waterfallBlock;

                    tempGrid[_tile.gridPos].TileGO = instance;

                    if (_tile.node != null)
                    {
                        GameObject nodeAsset = _tile.node;
                        GameObject nodeInstance = PrefabUtility.InstantiatePrefab(nodeAsset, gizmo.tileContent) as GameObject;
                        _tile.node = nodeInstance;
                    }
                }
            }

            if (_mapGroup == MapGroup.Factory)
            {
                levelData.grid = tempGrid;
                levelData.SaveGrid();
            }
        }

        void TerrainToggle(MapGroup _mapGroup = MapGroup.Null, bool forceActive = false)
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

        void SetCamera(MapGroup _mapGroup)
        {

            LevelVCamComponent cameraComponent = GetCamera(_mapGroup);
            TilemapGroup tilemapGroup = GetTilemapGroup(_mapGroup);

            //Set Bounds
            BoundsInt b = new BoundsInt();
            b.xMin = tileSize * (tilemapGroup.height.cellBounds.xMin + 1);
            b.xMax = tileSize * (tilemapGroup.height.cellBounds.xMax);
            b.yMin = (int)tilemapGroup.height.transform.position.y;
            b.yMax = (int)tilemapGroup.height.transform.position.y;
            b.zMin = tileSize * (tilemapGroup.height.cellBounds.yMin + 1);
            b.zMax = tileSize * (tilemapGroup.height.cellBounds.yMax);

            cameraComponent.cameraBounds = b;

            // Set Center
            cameraComponent.transform.position = b.center;
            cameraComponent.focusPoint.position = b.center;


            //Set FOV (Pas Précis, demande des ajustements manuels)

            float d = cameraComponent.cameraBounds.size.magnitude;
            float a = Mathf.Atan(250 * Mathf.Sqrt(2) / 230);
            float fov = 2 * Mathf.Atan((d * Mathf.Sin(a) / 2) / (Mathf.Sqrt(230 * 230 + 2 * 250 * 250) + d * 0.5f * Mathf.Sin(a) * Mathf.Tan(a)));
            fov *= Mathf.Rad2Deg;
            cameraComponent.cam.m_Lens.FieldOfView = fov;
            cameraComponent.farthestFOV = fov;
            cameraComponent.zoomedBounds = new Bounds(cameraComponent.cameraBounds.center, Vector3.zero);



        }

        #endregion

    }
}

