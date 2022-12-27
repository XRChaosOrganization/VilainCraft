using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileComponent))]
public class TileComponentEditor : Editor
{
    TileComponent tc;
    LevelData data;

    private void OnEnable()
    {
        data = FindObjectOfType<LevelData>();
    }
    public override void OnInspectorGUI()
    {

        tc = (TileComponent)target;
        base.OnInspectorGUI();
        EditorGUILayout.Space(10);

        if (data != null)
        {
            data.LoadGrid();
            EditorGUILayout.HelpBox("This Data is Read-Only. To edit Tile Data, please use the LevelPainter", MessageType.Info);
            GUI.enabled = false;
            ExtendedEditor.DrawTileInspector(data.grid[tc.gridPos]);
            GUI.enabled = true;
        }
        else EditorGUILayout.HelpBox("Must be in a Scene containing a LevelData instance to show Tile Details", MessageType.Info);

        EditorGUILayout.Space(15);
    }
}
