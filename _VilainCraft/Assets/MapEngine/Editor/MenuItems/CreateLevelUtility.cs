using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace MapEngineEditor
{
    public static class CreateLevelUtility
    {
        [MenuItem("GameObject/Custom/Empty Level")]
        public static void Create()
        {
            CreateUtility.CreatePrfab("Assets/MapEngine/Prefabs/Empty Level.prefab", false);
        }

    }
}

