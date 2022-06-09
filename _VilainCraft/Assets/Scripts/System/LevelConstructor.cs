using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelConstructor : MonoBehaviour
{
    public int tileSize;

    public GameObject tilePrefab;
    public Transform tileContainer;
    public Vector2 terrainSize;
    [Range(0, 100)] public int variance;


    public void GenerateTerrain()
    {
        for (int x = 0; x < terrainSize.x; x++)
        {
            for (int y = 0; y < terrainSize.y; y++)
            {
                Vector3 position = new Vector3(x * tileSize, 0f, y * tileSize);

                int direction = Random.Range(0, 4);
                float varianceCheck = Random.Range(0f, 1f);
                int variantIndex = Random.Range(1, 4);

                GameObject instance = PrefabUtility.InstantiatePrefab(tilePrefab, tileContainer) as GameObject;
                instance.transform.position = position;
                instance.transform.rotation = Quaternion.Euler(0f, 90f * direction, 0f);

                if (varianceCheck <= (float)variance / 100)
                {
                    MeshRenderer[] mr = instance.GetComponentsInChildren<MeshRenderer>();
                    mr[variantIndex].enabled = true;
                }


            }
        }
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
}


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
        {
            constructor.GenerateTerrain();
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Terrain"))
        {
            constructor.ClearTerrain();
        }
        
    }
}


