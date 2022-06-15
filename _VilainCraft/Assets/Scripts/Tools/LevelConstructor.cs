using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CellData
{
    public Vector3 position;
    public int height;

    public CellData(Vector3 _pos, int _height)
    {
        position = _pos;
        height = _height;
    }
}


public class LevelConstructor : MonoBehaviour
{
    public int tileSize;

    //public GameObject[] tilePrefabs;
    public Tileset tileset;
    public Transform tileContainer;
    [HideInInspector] public Texture2D mapData;

    [Range(0, 100)] public int variance;

    #region Public Methods
    public void GenerateTerrain()
    {
        if (tileContainer.childCount > 0)
            ClearTerrain();

        List<CellData> _mapData = ReadMapData();
        PlaceTiles(_mapData);
        
    }

    public void ClearTerrain()
    {
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
        List<CellData> _data = new List<CellData>();
        for (int x = 0; x < mapData.width; x++)
        {
            for (int y = 0; y < mapData.height; y++)
            {
                Vector3 position = new Vector3(x * tileSize, 0f, y * tileSize);

                Color color = mapData.GetPixel(x, y);
                int step = 256 / (tileset.terrain.Count + 1);
                int height = (int)(color.r *256 / step);

                _data.Add(new CellData(position, height));

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

        foreach (var item in _data)
        {
            if (item.height > 0)
            {
                int direction = Random.Range(0, 4);
                float varianceCheck = Random.Range(0f, 1f);
                int variantIndex = Random.Range(1, 4);

                
                GameObject instance = PrefabUtility.InstantiatePrefab(tileset.terrain[item.height - 1], tileContainer) as GameObject;
                instance.transform.position = item.position;
                instance.transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);

                if (varianceCheck <= (float)variance / 100)
                {
                    MeshRenderer[] mr = instance.GetComponentsInChildren<MeshRenderer>();
                    mr[variantIndex].enabled = true;
                }
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
       
        constructor.mapData = (Texture2D)EditorGUILayout.ObjectField("Map Data", constructor.mapData, typeof(Texture2D), false);

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
