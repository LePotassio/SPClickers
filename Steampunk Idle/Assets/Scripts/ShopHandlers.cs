using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class ShopHandlers : MonoBehaviour
{
    public GameManager gm;

    public Button scuttlerButton;

    public Button haulerButton;

    //Just pretend this is golem
    int scuttlerCost = 100;

    int haulerCost = 50;

    public CollectionUpdater collectionUpdater;

    private void Update()
    {
        if (gm.Scrap >= haulerCost && !gm.blueprints[1])
        {
            haulerButton.interactable = true;
        } else
        {
            haulerButton.interactable = false;
        }

        if (gm.Scrap >= scuttlerCost && !gm.blueprints[0])
        {
            scuttlerButton.interactable = true;
        } else
        {
            scuttlerButton.interactable = false;
        }
    }

    public void onScuttler()
    {
        if (gm.Scrap >= scuttlerCost)
        {
            gm.Scrap -= scuttlerCost;
            gm.resourceHUD.UpdateHUD(gm);
            gm.blueprints[0] = true;
            scuttlerButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Bought";
            collectionUpdater.updateBlueprints();
        }
    }

    public void onHauler()
    {
        if (gm.Scrap >= haulerCost) {
            gm.Scrap -= haulerCost;
            gm.resourceHUD.UpdateHUD(gm);
            gm.blueprints[1] = true;
            haulerButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Bought";
            collectionUpdater.updateBlueprints();
        }
    }
}
