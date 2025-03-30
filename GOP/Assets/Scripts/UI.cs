using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI discontentmentText; 
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI drivingStyle;

    public void UpdateActionUI(string goaltext, string action)
    {
        discontentmentText.text = "Stats: " + goaltext; //discontentment.ToString("F2");
        actionText.text = "Action: " + action;
    }

    public void UpdateToggleUI(bool driveBool)
    {
        drivingStyle.text = "Safe Driving Mode: " + (driveBool ? "ON" : "OFF");
    }
    public void UpdateStatsUI(string goaltext)
    {
        discontentmentText.text = "Stats: " + goaltext;
    }
}
