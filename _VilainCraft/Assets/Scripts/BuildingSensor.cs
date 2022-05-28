using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSensor : MonoBehaviour
{
    public BuildingComponent parentBuildingComponent;

    private void Start()
    {
        parentBuildingComponent = GetComponentInParent<BuildingComponent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            parentBuildingComponent.buildable = false;
        }
    }
}
