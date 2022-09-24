using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
