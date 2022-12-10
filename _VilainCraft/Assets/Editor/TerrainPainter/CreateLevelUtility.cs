using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public static class CreateLevelUtility 
{
    [MenuItem("GameObject/Custom/Empty Level")]
    public static void Create()
    {
        CreateUtility.CreatePrfab("Assets/Prefabs/System/Empty Level.prefab", false);
    }

}
