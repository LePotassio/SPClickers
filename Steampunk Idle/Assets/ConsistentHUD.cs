using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ConsistentHUD : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;

    public void UpdateHUD(GameManager gm)
    {
        timeText.text = "Day: " + gm.Day + "\nTime: " + gm.Hour + ":00";
        healthText.text = "Health: " + gm.playerShipStats.health + "/" + gm.playerShipStats.maxHealth;
    }
}
