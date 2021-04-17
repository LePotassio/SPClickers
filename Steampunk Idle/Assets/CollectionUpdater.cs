using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CollectionUpdater : MonoBehaviour
{
    public TextMeshProUGUI blueprintText;
    public TextMeshProUGUI botsText;
    public GameManager gm;

    public void updateBlueprints()
    {
        string result = "";

        if (gm.blueprints[0] && gm.blueprints[1])
            result = "Scrap Scuttler, Scrap Hauler";
        else if (gm.blueprints[0])
            result = "Scrap Scuttler";
        else if (gm.blueprints[1])
            result = "Scrap hauler";
        
        blueprintText.text = result;
    }

    public void updateBots()
    {
        string result = "";

        if (gm.robots[0] > 0)
        {
            result += "Scrap Scuttlers: " + gm.robots[0] + "\n";
        }

        if (gm.robots[1] > 0)
        {
            result += "Scrap Haulers: " + gm.robots[1];
        }

        botsText.text = result;
    }
}
