using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ConsistentHUD : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI storyText;
    public List<string> storyList;

    public void UpdateHUD(GameManager gm)
    {
        timeText.text = "Day: " + gm.Day + "\nTime: " + gm.Hour + ":00";
        healthText.text = "HP: " + gm.playerShipStats.health + "/" + gm.playerShipStats.maxHealth;
    }

    public void addStory(string story)
    {
        storyList.Insert(0, story);
        if (storyList.Count > 3)
        {
            storyList.RemoveAt(storyList.Count - 1);
        }

        string res = "";

        for (int i = storyList.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                string colorized = "<color=black>" + "->" + storyList[i] + "</color>" + "\n";
                res += colorized;
                break;
            }
            res += "<color=grey>" + "->" + storyList[i] + "</color>" + "\n";
        }

        storyText.text = res;
    }
}
