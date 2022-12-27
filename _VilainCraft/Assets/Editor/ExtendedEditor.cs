using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ExtendedEditor
{
    public static void DrawTileInspector(Tile _tile)
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tile", EditorStyles.boldLabel);

        EditorGUILayout.Vector2Field("Grid Position", _tile.gridPos);
        EditorGUILayout.EnumFlagsField("Tile Type", _tile.type);
        EditorGUILayout.ObjectField("Associated GO", _tile.associatedGO, typeof(GameObject), true);
        EditorGUILayout.IntField("Height", _tile.height);
        EditorGUILayout.Toggle("Is Blocked", _tile.isBlocked);

        EditorGUILayout.Separator();

        EditorGUILayout.Space(2);
        EditorGUILayout.LabelField("Content", EditorStyles.boldLabel);
        EditorGUILayout.ObjectField("Building", _tile.building, typeof(GameObject), true);
        EditorGUILayout.ObjectField("Node", _tile.node, typeof(RawRessource), true);

        


        EditorGUILayout.EndVertical();

    }
}
