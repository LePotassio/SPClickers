using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class NaviHandler : MonoBehaviour
{
    public Button departButton;
    public TextMeshProUGUI destTime;
    public GameManager gm;

    public GameObject naviInFlight;
    public GameObject naviArrived;

    public void OnDepart()
    {
        gm.timeToShop = 30;
        gm.gameState = GameManager.GameState.IDLE;
        gm.mh.sbutton.GetComponent<Button>().interactable = false;
        gm.nh.departButton.interactable = false;
        gm.gear.GetComponent<SpriteRenderer>().enabled = true;
        DestroyImmediate(gm.shopSprite, false);

        naviInFlight.SetActive(true);
        naviArrived.SetActive(false);
    }
}
