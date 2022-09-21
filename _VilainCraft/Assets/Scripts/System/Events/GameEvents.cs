using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    #region Events

    public event Action<bool> onDisplayGrid;
    public void DisplayGrid(bool b)
    {
        if (onDisplayGrid != null)
        {
            onDisplayGrid(b);
        }
    }




    #endregion
}
