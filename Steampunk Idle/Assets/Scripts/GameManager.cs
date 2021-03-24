using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { IDLE, ACTIVE, EVENT, SHOP, BATTLE }

    public GameState gs = GameState.IDLE;

    private int scrap = 0;
    private int scrapPC = 1;
    private int scrapPS = 0;

    public ResourceHUD resourceHUD;
    public DebugHUD debugHUD;

    public int Scrap
    {
        get
        {
            return scrap;
        }
        set
        {
            scrap = value;
        }
    }

    public int ScrapPC
    {
        get
        {
            return scrapPS;
        }
        set
        {
            scrapPS = value;
        }
    }

    public int ScrapPS
    {
        get
        {
            return scrapPS;
        }
        set
        {
            scrapPS = value;
        }
    }

    public GameState gameState
    {
        get
        {
            return gs;
        }
        set
        {
            gs = value;
            debugHUD.UpdateHUD(this);
        }
    }

    private void Start()
    {
        resourceHUD.UpdateHUD(this);
        InvokeRepeating("IncrementTime", 0, 1);
    }

    private void IncrementTime()
    {
        if (gameState == GameState.IDLE)
        {
            // Only passive resource gain
            IncrementResources();
        }
        else if (gameState == GameState.ACTIVE) {
            // Events and battles now randomly occur
        }
        else if (gameState == GameState.BATTLE)
        {
            // Start an auto battle
        }
        else if (gameState == GameState.EVENT)
        {
            // Give the player a text prompt and pause resource updates
        }

        // resourceHUD.UpdateHUD(this);
    }

    private void IncrementResources()
    {
        scrap += scrapPS;
        resourceHUD.UpdateHUD(this);
    }

    public void OnScrapButton()
    {
        scrap += scrapPC;
        resourceHUD.UpdateHUD(this);
    }

    public void OnIdleToggleButton()
    {
        if (gameState == GameState.IDLE)
        {
            gameState = GameState.ACTIVE;
        }
        else if(gameState == GameState.ACTIVE)
        {
            gameState = GameState.IDLE;
        }
    }
}
