using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;


public class CellData
{
    public enum Type { Terrain, Water }
    

    public Vector3 position;
    public int height;
    public Type type;
    public bool isBuildable;


    public Dictionary<string, bool> waterCullingMask = new Dictionary<string, bool>();
    
    

    public CellData(Vector3 _pos, int _height, Type _type)
    {
        position = _pos;
        height = _height;
        type = _type;
        isBuildable = type == Type.Water ? true : false;
        
    }


}

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

    [Range(0, 100)] public int variance;

    #region Public Methods
    public void GenerateTerrain()
    {
        Debug.ClearDeveloperConsole();
        if (tileContainer.childCount > 0)
            ClearTerrain();

        List<CellData> _mapData = ReadMapData();
        PlaceTiles(_mapData);
        
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

    #endregion


    #region Map Generation

    List<CellData> ReadMapData()
    {

        Texture2D texture = mapData.heightMap.texture;
        Rect heightRect = mapData.heightMap.rect;
        Rect waterRect = mapData.waterMap.rect;


        List<CellData> _data = new List<CellData>();
        for (int x = 0; x < heightRect.width; x++)
        {
            for (int y = 0; y < heightRect.height; y++)
            {
                Vector3 position = new Vector3(x * tileSize, 0f, y * tileSize);

                Color color = texture.GetPixel(x + (int)heightRect.x, y + (int)heightRect.y);
                int step = 256 / (tileset.terrain.Count + 1);
                int height = (int)(color.r *256 / step);

                CellData _cellData = new CellData(
                    position,
                    height,
                    texture.GetPixel(x + (int)waterRect.x, y + (int)waterRect.y).b >= 0.3 ? CellData.Type.Water : CellData.Type.Terrain
                    );

                if (_cellData.type == CellData.Type.Water)
                {
                    int faces = 0;

                    _cellData.waterCullingMask.Add("X-", (int)(texture.GetPixel(x + (int)heightRect.x - 1, y + (int)heightRect.y).r * 256 / step) == 0);
                    faces += (int)(texture.GetPixel(x + (int)heightRect.x - 1, y + (int)heightRect.y).r * 256 / step) == 0 ? 1 : 0 ;
                    _cellData.waterCullingMask.Add("X+", (int)(texture.GetPixel(x + (int)heightRect.x + 1, y + (int)heightRect.y).r * 256 / step) == 0);
                    faces += (int)(texture.GetPixel(x + (int)heightRect.x + 1, y + (int)heightRect.y).r * 256 / step) == 0 ? 1 : 0;
                    _cellData.waterCullingMask.Add("Z-", (int)(texture.GetPixel(x + (int)heightRect.x, y + (int)heightRect.y - 1).r * 256 / step) == 0);
                    faces += (int)(texture.GetPixel(x + (int)heightRect.x, y + (int)heightRect.y -1).r * 256 / step) == 0 ? 1 : 0;
                    _cellData.waterCullingMask.Add("Z+", (int)(texture.GetPixel(x + (int)heightRect.x, y + (int)heightRect.y + 1).r * 256 / step) == 0);
                    faces += (int)(texture.GetPixel(x + (int)heightRect.x, y + (int)heightRect.y +1).r * 256 / step) == 0 ? 1 : 0;

                    Debug.Log(faces);

                }

                _data.Add(_cellData);

            }
        }

        return _data;
    }



    void PlaceTiles(List<CellData> _data)
    {
        if (tileset.terrain.Count == 0)
        {
            Debug.LogError(tileset.name + ".terrain is empty !");
            return;
        }

        for (int i = 0; i < _data.Count; i++)
        {
            if (_data[i].height > 0)
            {
                int direction = Random.Range(0, 4);
                float varianceCheck = Random.Range(0f, 1f);
                int variantIndex = Random.Range(0, 3);
                GameObject instance;

                switch (_data[i].type)
                {
                    case CellData.Type.Terrain:
                        instance = PrefabUtility.InstantiatePrefab(tileset.terrain[_data[i].height - 1], tileContainer) as GameObject;
                        instance.transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);
                        break;

                    case CellData.Type.Water:
                        instance = PrefabUtility.InstantiatePrefab(tileset.water[_data[i].height - 1], tileContainer) as GameObject;
                        GenerateWaterBorders(_data[i], instance);       
                        break;

                    default:
                        instance = null;
                        Debug.LogError("Tile " + _data[i].position + " Type not recognized");
                        return;

                }

                instance.transform.position = _data[i].position;
                

                if (varianceCheck <= (float)variance / 100)
                {
                    Transform container = instance.transform.Find("Variants");
                    if (container != null)
                    {
                        MeshRenderer[] mr = container.GetComponentsInChildren<MeshRenderer>();
                        mr[variantIndex].enabled = true;
                    }
                    
                }
            }

        }
    }

    void GenerateWaterBorders(CellData _item, GameObject _instance)
    {
        foreach (var valuePair in _item.waterCullingMask)
        {
            if (valuePair.Value)
            {
                Transform face = _instance.transform.Find(valuePair.Key);
                face.GetComponent<MeshRenderer>().enabled = true;
            }
        }
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
