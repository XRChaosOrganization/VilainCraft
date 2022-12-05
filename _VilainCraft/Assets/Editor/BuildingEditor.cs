using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{


    public IDictionary<Vector2, Tile.Tile_Type> e_requiredTiles = new Dictionary<Vector2, Tile.Tile_Type>();

    Color SetButtonColor(Vector2 _key)
    {
        Color c = new Color();

        switch (e_requiredTiles[_key])
        {
            case Tile.Tile_Type.Void:
                c = Color.white;
                break;
            case Tile.Tile_Type.Grass:
                c = Color.green;
                break;
            case Tile.Tile_Type.Water:
                c = Color.cyan;
                break;
            default:
                c = Color.white;
                break;
        }

        return c;
    }




    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Building building = (Building)target;
        GUILayout.Space(14);
        GUILayout.Label("Required Tiles");
        Color c = GUI.backgroundColor;



        foreach (Pos_Type_Pair pair in building.requiredTiles)
            e_requiredTiles[pair.gridPos] = pair.tileType;


        GUILayout.BeginHorizontal();
        for (int x = -2; x < 3; x++)
        {
            GUILayout.BeginVertical();
            for (int y = 2; y > -3; y--)
            {
                Vector2 _key = new Vector2(x, y);
                e_requiredTiles.TryAdd(_key, Tile.Tile_Type.Void);
                GUI.backgroundColor = SetButtonColor(_key);
                if (GUILayout.Button(Mathf.RoundToInt(_key.x).ToString() + " , " + Mathf.RoundToInt(_key.y).ToString(), GUILayout.Width(50), GUILayout.Height(50)))
                {
                    int index = (int)e_requiredTiles[_key];
                    index = (index + 1) % 3;
                    e_requiredTiles[_key] = (Tile.Tile_Type)index;
                }
                GUILayout.Space(8);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        GUI.backgroundColor = c;
        if (building.requiredTiles.Count < e_requiredTiles.Count)
        {
            building.requiredTiles.Clear();
            foreach (KeyValuePair<Vector2, Tile.Tile_Type> pair in e_requiredTiles)
                building.requiredTiles.Add(new Pos_Type_Pair(pair.Key, pair.Value));
        }
        else
            foreach (Pos_Type_Pair pair in building.requiredTiles)
                pair.tileType = e_requiredTiles[pair.gridPos];
    }
}
