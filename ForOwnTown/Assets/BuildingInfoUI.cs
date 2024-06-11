using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoUI : MonoBehaviour
{
    public Text buildingInfoText;

    public void UpdateBuildingInfo(string buildingName, int buildingLevel, string actions, string resources)
    {
        buildingInfoText.text = $"{buildingName} Level: {buildingLevel}\nActions:\n{actions}\nResources:\n{resources}";
    }

    public void UpdateWallInfo(string wallType, int wallLevel, string actions, string resources)
    {
        buildingInfoText.text = $"Wall Type: {wallType} Level: {wallLevel}\nActions:\n{actions}\nResources:\n{resources}";
    }

    public void UpdateWatchtowerInfo(int watchtowerLevel, string actions, string resources)
    {
        buildingInfoText.text = $"Watchtower Level: {watchtowerLevel}\nActions:\n{actions}\nResources:\n{resources}";
    }
}
