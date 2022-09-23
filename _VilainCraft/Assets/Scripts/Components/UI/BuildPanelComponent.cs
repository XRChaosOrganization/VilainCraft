using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildPanelComponent : MonoBehaviour
{
    public TextMeshProUGUI buildingGroupName;
    public List<GameObject> buttons;
    /*[HideInInspector]*/ public List<GameObject> buildingList;

    public void UpdatePanel()
    {
        foreach (var button in buttons)
            button.SetActive(true);

        for (int i = 0; i < buildingList.Count; i++)
        {
            Building building = buildingList[i].GetComponent<BuildingBehavior>().building;
            buttons[i].GetComponent<Image>().sprite = building.sprite;
            buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = building.name;
            buttons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = building.cost.ToString();
        }

        for (int j = buildingList.Count; j < buttons.Count; j++)
            buttons[j].SetActive(false);

    }

    public void SetAnimatorState(bool _isOpen)
    {
        GetComponent<Animator>().SetBool("isOpen", _isOpen);
    }

    public void OnButtonClick(int _id)
    {
        BuildingHandler.current.SetBuildingStamp(buildingList[_id]);
    }
}
