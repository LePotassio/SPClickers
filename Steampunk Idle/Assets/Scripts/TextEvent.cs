using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEvent : MonoBehaviour
{
    public string eventText;
    public string eventTextShort;

    public int numOptions; // max 2

    public int scrapOption1;
    public int healthOption1;
    public string option1Name;
    public string option1Text;

    public int scrapOption2;
    public int healthOption2;
    public string option2Name;
    public string option2Text;


    public int scrapDoNothing;
    public int healthDoNothing;
    public string doNothingText;

    //Lots could be better here, could search for gm and could also pull from prefabs...
    public GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    //Ideally these would be virtual for full implementation
    public void onOption1()
    {
        // Set consistent story text and add scrap
        string optionText = option1Text;

        if (scrapOption1 > 0)
            optionText += "<color=green>" + " Scrap +" + scrapOption1 + "</color>";
        else if (scrapOption1 < 0)
            optionText += "<color=red>" + " Scrap " + scrapOption1 + "</color>";

        if (healthOption1 > 0)
            optionText += "<color=green>" + " Health +" + healthOption1 + "/color";
        else if (healthOption1 < 0)
            optionText += "<color=red>" + " Health " + healthOption1 + "</color>";

        gm.consistentHUD.addStory(optionText);
        gm.Scrap += scrapOption1;
        gm.playerShipStats.health += healthOption1;

        if (gm.Scrap < 0)
        {
            gm.Scrap = 0;
        }

        if (gm.playerShipStats.health > gm.playerShipStats.maxHealth)
        {
            gm.playerShipStats.health = gm.playerShipStats.maxHealth;
        }

        if (gm.playerShipStats.health <= 0)
        {
            gm.playerShipStats.health = gm.playerShipStats.maxHealth;
            gm.consistentHUD.addStory("Your ship must repair and you lose half your scrap...");
            gm.consistentHUD.UpdateHUD(gm);
            gm.debugHUD.UpdateHUD(gm);
            gm.resourceHUD.UpdateHUD(gm);
        }

        gm.resourceHUD.UpdateHUD(gm);

        gm.eventCanvas.enabled = false;
        gm.gameState = GameManager.GameState.IDLE;
        StartCoroutine(gm.EventCDTimer());
    }

    public void onOption2()
    {
        // Set consistent story text and add scrap

        string optionText = option2Text;

        if (scrapOption2 > 0)
            optionText += "<color=green>" + " Scrap +" + scrapOption2 + "</color>";
        else if (scrapOption2 < 0)
            optionText += "<color=red>" + " Scrap " + scrapOption2 + "</color>";

        if (healthOption2 > 0)
            optionText += "<color=green>" + " Health +" + healthOption2 + "</color>";
        else if (healthOption2 < 0)
            optionText += "<color=red>" + " Health " + healthOption2 + "</color>";

        gm.consistentHUD.addStory(optionText);
        gm.Scrap += scrapOption2;
        gm.playerShipStats.health += healthOption2;

        if (gm.Scrap < 0)
        {
            gm.Scrap = 0;
        }

        if (gm.playerShipStats.health > gm.playerShipStats.maxHealth)
        {
            gm.playerShipStats.health = gm.playerShipStats.maxHealth;
        }

        gm.resourceHUD.UpdateHUD(gm);

        if (gm.playerShipStats.health <= 0)
        {
            gm.playerShipStats.health = gm.playerShipStats.maxHealth;
            gm.consistentHUD.addStory("Your ship must repair and you lose half your scrap...");
            gm.consistentHUD.UpdateHUD(gm);
            gm.debugHUD.UpdateHUD(gm);
            gm.resourceHUD.UpdateHUD(gm);
        }

        gm.eventCanvas.enabled = false;
        gm.gameState = GameManager.GameState.IDLE;
        StartCoroutine(gm.EventCDTimer());
    }

    public void onDoNothing()
    {
        string optionText = doNothingText;

        if (scrapDoNothing > 0)
            optionText += "<color=green>" + " Scrap +" + scrapDoNothing + "</color>";
        else if (scrapOption2 < 0)
            optionText += "<color=red>" + " Scrap " + scrapDoNothing + "</color>";

        if (healthDoNothing > 0)
            optionText += "<color=green>" + " Health +" + healthDoNothing + "</color>";
        else if (healthDoNothing < 0)
            optionText += "<color=red>" + " Health " + healthDoNothing + "</color>";

        gm.consistentHUD.addStory(optionText);
        gm.Scrap += scrapDoNothing;
        gm.playerShipStats.health += healthDoNothing;

        if (gm.Scrap < 0)
        {
            gm.Scrap = 0;
        }

        if (gm.playerShipStats.health > gm.playerShipStats.maxHealth)
        {
            gm.playerShipStats.health = gm.playerShipStats.maxHealth;
        }

        gm.resourceHUD.UpdateHUD(gm);

        if (gm.playerShipStats.health <= 0)
        {
            gm.playerShipStats.health = gm.playerShipStats.maxHealth;
            gm.consistentHUD.addStory("Your ship must repair and you lose half your scrap...");
            gm.consistentHUD.UpdateHUD(gm);
            gm.debugHUD.UpdateHUD(gm);
            gm.resourceHUD.UpdateHUD(gm);
        }
    }
}
