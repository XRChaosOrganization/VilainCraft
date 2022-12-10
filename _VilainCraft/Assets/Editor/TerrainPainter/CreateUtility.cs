using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

public static class CreateUtility 
{
    public static void CreatePrfab(string path, bool asPrefab = true)
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        GameObject newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Place(newObject);
        if (!asPrefab)
            PrefabUtility.UnpackPrefabInstance(newObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
    }

    public static void CreateObject(string name, params Type[] types)
    {
        GameObject newObject = ObjectFactory.CreateGameObject(name, types);
        Place(newObject);
    }


    public static void Place(GameObject _gameObject)
    {
        _gameObject.transform.position = Vector3.zero;

        StageUtility.PlaceGameObjectInCurrentStage(_gameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(_gameObject);

        Undo.RegisterCreatedObjectUndo(_gameObject, $"Create Object: {_gameObject.name}");
        Selection.activeGameObject = _gameObject;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
