using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MouseHandler))]
public class MouseHandlerEditor : Editor
{
    MouseHandler handler;
    
    bool tileMOFoldout = true;


    

    public override void OnInspectorGUI()
    {
        
        handler = (MouseHandler)target;
        base.OnInspectorGUI();

        GUI.enabled = false;

        EditorGUILayout.Space(20);
        EditorGUILayout.Vector3Field("Screen Point", handler.screenPoint);
        //Add Field For World Point

        EditorGUILayout.Space(20);
        GUI.enabled = true;



        EditorGUI.indentLevel++;
        tileMOFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(tileMOFoldout, "Moused Over Tile Infos");
        if (tileMOFoldout)
        {
            GUI.enabled = false;
            Vector3 worldPos = handler.tile_mo.worldPos;
            EditorGUILayout.Vector3Field("World Position", worldPos);

            Vector3 tileAnchor = handler.tile_mo.tileAnchor;
            EditorGUILayout.Vector3Field("Tile Anchor", tileAnchor);

            ExtendedEditor.DrawTileInspector(handler.tile_mo.tile);
            GUI.enabled = true;

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.indentLevel--;


        //Add Section For Building (+ one other for Units / Towers ?)


        GUI.enabled = true;
    }
}
