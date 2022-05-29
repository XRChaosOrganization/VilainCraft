using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSensor : MonoBehaviour
{
    public BuildingComponent parentBuildingComponent;
    public List<BuildingSensor> relatedSensors;
    public Transform sensorsContainer;

    private void Start()
    {
        parentBuildingComponent = GetComponentInParent<BuildingComponent>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            parentBuildingComponent.buildable = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other!=null)
        {
            parentBuildingComponent.buildable = true;
        }
    }
    private void OnMouseOver()
    {
        if (UIManager.current.isDemolishMode)
        {
            for (int i = 0; i < parentBuildingComponent.buildingMeshs.Count; i++)
            {
                parentBuildingComponent.buildingMeshs[i].material.color = Color.yellow;
            }
        }
    }
    private void OnMouseDown()
    {
        if (UIManager.current.isDemolishMode)
        {
            for (int i = 0; i < relatedSensors.Count; i++)
            {
                UIManager.current.colliders.Remove(relatedSensors[i]);
            }
            Destroy(parentBuildingComponent.gameObject);
        }
    }
    private void OnMouseExit()
    {
        if (UIManager.current.isDemolishMode)
        {
            for (int i = 0; i < parentBuildingComponent.buildingMeshs.Count; i++)
            {
                parentBuildingComponent.buildingMeshs[i].material.color = Color.white;
            }
        }
    }
}
