using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Transform tileContainer;
    public static GameManager gm;
    public List<TileComponent> tilesList;

    private void Awake()
    {
        gm = this;
        tilesList = new List<TileComponent>();
        
        foreach(Transform child in tileContainer)
        {
            tilesList.Add(child.GetComponent<TileComponent>());
        }
    }

}
