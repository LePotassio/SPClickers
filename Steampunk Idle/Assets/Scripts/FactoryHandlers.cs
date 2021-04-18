using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class FactoryHandlers : MonoBehaviour
{
    public GameManager gm;
    public Button scuttlerBuildButton;
    public Button haulerBuildButton;
    public Button smallScuttleBuildButton;

    public int scuttlerBuildPrice = 100;

    public int haulerBuildPrice = 50;

    public int smallScuttleBuildPrice = 10;

    public TextMeshProUGUI scuttlerName;
    public TextMeshProUGUI haulerName;
    public TextMeshProUGUI smallScuttleName;

    public CollectionUpdater collectionUpdater;

    public void Update()
    {
        //Lol this is bad but oh well
        if (gm.blueprints[0])
        {
            scuttlerBuildButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Build";
        }

        if (gm.blueprints[1])
        {
            haulerBuildButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Build";
        }

        if (gm.Scrap >= scuttlerBuildPrice && gm.blueprints[0])
        {
            scuttlerBuildButton.interactable = true;
        } else
        {
            scuttlerBuildButton.interactable = false;
        }

        if (gm.Scrap >= haulerBuildPrice && gm.blueprints[1])
        {
            haulerBuildButton.interactable = true;
        }
        else
        {
            haulerBuildButton.interactable = false;
        }

        if (gm.Scrap >= smallScuttleBuildPrice)
        {
            smallScuttleBuildButton.interactable = true;
        }
        else
        {
            smallScuttleBuildButton.interactable = false;
        }
    }

    public void onBuildScuttler()
    {
        if (gm.Scrap >= scuttlerBuildPrice && gm.blueprints[0])
        {
            gm.Scrap -= scuttlerBuildPrice;
            gm.ScrapPS += 10;
            gm.robots[0]++;
            gm.resourceHUD.UpdateHUD(gm);
            scuttlerName.text = "(" + gm.robots[0] + ")Scrap Golem (Cost:100)";
            collectionUpdater.updateBots();
        }
    }

    public void onBuildHauler()
    {
        if (gm.Scrap >= haulerBuildPrice && gm.blueprints[0])
        {
            gm.Scrap -= haulerBuildPrice;
            gm.ScrapPS += 5;
            gm.robots[1]++;
            gm.resourceHUD.UpdateHUD(gm);
            haulerName.text = "(" + gm.robots[1] + ")Scrap Hauler (Cost:50)";
            collectionUpdater.updateBots();
        }
    }

    public void onBuildSmallScuttler()
    {
        if (gm.Scrap >= smallScuttleBuildPrice)
        {
            gm.Scrap -= smallScuttleBuildPrice;
            gm.ScrapPS += 1;
            gm.robots[2]++;
            gm.resourceHUD.UpdateHUD(gm);
            smallScuttleName.text = "(" + gm.robots[2] + ")Scrap Scuttler (Cost:10)";
            collectionUpdater.updateBots();
        }
    }
}
