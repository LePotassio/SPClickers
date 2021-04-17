using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ResourceHUD : MonoBehaviour
{
    public TextMeshProUGUI scrapText;
    public TextMeshProUGUI scrapPSText;
    public TextMeshProUGUI scrapButtonCDText;

    public void UpdateHUD(GameManager gm)
    {
        scrapText.text = "--" + gm.Scrap + "--";
        scrapPSText.text = gm.ScrapPS + " sps";
        if (gm.ScrapButtonCD <= 0)
            //scrapButtonCDText.text = "Ready";
            scrapButtonCDText.text = "";
        else
            //scrapButtonCDText.text = "Cooling down\n(" + gm.ScrapButtonCD + " seconds)";
            scrapButtonCDText.text = "";
    }
}
