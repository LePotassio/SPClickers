using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ResourceHUD : MonoBehaviour
{
    public TextMeshProUGUI scrapText;
    public TextMeshProUGUI scrapPSText;


    public void UpdateHUD(GameManager gm)
    {
        scrapText.text = "Scrap: " + gm.Scrap;
        scrapPSText.text = "Scrap Per Second: " + gm.ScrapPS;
    }
}
