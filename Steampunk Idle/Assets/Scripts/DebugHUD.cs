using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DebugHUD : MonoBehaviour
{
    public TextMeshProUGUI stateText;

    public void UpdateHUD(GameManager gm)
    {
        stateText.text = "Current Game State: " + gm.gameState;
    }
}
