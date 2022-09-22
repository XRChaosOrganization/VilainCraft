using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildBarButtonComponent : MonoBehaviour
{
    public BuildPanelComponent buildPanel;
    public string groupName;
    public List<GameObject> buildingList;

    public void OnClick()
    {
        if (buildPanel.buildingGroupName.text == groupName && buildPanel.GetComponent<Animator>().GetBool("isOpen"))
        {
            buildPanel.SetAnimatorState(false);
            return;
        }

        buildPanel.buildingGroupName.text = groupName;
        buildPanel.buildingList = buildingList;
        buildPanel.UpdatePanel();
        buildPanel.SetAnimatorState(true);

        
    }
}
