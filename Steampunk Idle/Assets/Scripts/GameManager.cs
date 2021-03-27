using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Gamestate Stuff
    public enum GameState { IDLE, ACTIVE, EVENT, SHOP, BATTLE, PAUSE }

    public GameState gs = GameState.IDLE;

    // Resource Tracking Stuff
    private int scrap = 0;
    private int scrapPC = 1;
    private int scrapPS = 0;
    private float scrapButtonCD = 0f;
    private float scrapButtonCDMax = 20f;


    // Time Stuff
    // 1 hr per second
    private int passiveTimeIncrement = 1;
    // 1 subhr per second (then round down at the end of battle)
    private int activeTimeIncrement = 1;

    // Number of subhours seconds that equal an hour
    private int subHourSeconds = 10;
    private int subHour = 0;
    private int hour = 0;
    private int day = 0;

    // Battle stuff
    // This could be a zone class later
    public List<GameObject> currentZoneEnemies;
    public Transform enemySpawn;
    public ShipStats playerShipStats;
    public ShipStats enemyShipStats;

    public GameObject placeholderPirate;

    // HUD Stuff
    public ResourceHUD resourceHUD;
    public DebugHUD debugHUD;
    public ConsistentHUD consistentHUD;

    public Canvas resourceScreen;
    public Canvas debugInfo;

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

    public float ScrapButtonCD
    {
        get
        {
            return scrapButtonCD;
        }
        set
        {
            scrapButtonCD = value;
        }
    }

    public float ScrapButtonCDMax
    {
        get
        {
            return scrapButtonCDMax;
        }
        set
        {
            scrapButtonCDMax = value;
        }
    }

    public int Hour
    {
        get
        {
            return hour;
        }
        set
        {
            hour = value;
        }
    }

    public int Day
    {
        get
        {
            return day;
        }
        set
        {
            day = value;
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
        debugHUD.UpdateHUD(this);
        resourceHUD.UpdateHUD(this);
        InvokeRepeating("IncrementTime", 0, 1);
        InvokeRepeating("IncrementBattle", 0, .1f);
    }

    private void IncrementTime()
    {
        if (gameState == GameState.IDLE)
        {
            // Only passive resource gain
            IncrementResources();
            IncrementTimer(passiveTimeIncrement);
        }
        else if (gameState == GameState.ACTIVE) {
            // Events and battles now randomly occur
            IncrementTimerActive(activeTimeIncrement);
            RandomEncounterTick();
        }
        else if (gameState == GameState.BATTLE)
        {
            // This is done in IncrementBattle()
        }
        else if (gameState == GameState.EVENT)
        {
            // Give the player a text prompt and pause resource updates
        }

        // resourceHUD.UpdateHUD(this);
    }

    private void IncrementTimer(int hours)
    {
        hour += hours;
        day += hour / 24;
        hour %= 24;

        consistentHUD.UpdateHUD(this);
    }

    private void IncrementTimerActive(int subHours)
    {
        subHour += subHours;
        hour += subHour / subHourSeconds;
        subHour %= subHourSeconds;

        day += hour / 24;
        hour %= 24;

        consistentHUD.UpdateHUD(this);
    }

    private void IncrementResources()
    {
        scrap += scrapPS;

        if (scrapButtonCD > 0)
            scrapButtonCD -= 1f;
        resourceHUD.UpdateHUD(this);
    }

    public void OnScrapButton()
    {
        if (scrapButtonCD <= 0)
        {
            scrap += scrapPC;

            scrapButtonCD = scrapButtonCDMax;

            resourceHUD.UpdateHUD(this);
        }
    }

    public void OnIdleToggleButton()
    {
        if (gameState == GameState.IDLE)
        {
            gameState = GameState.ACTIVE;
        }
        else if(gameState == GameState.ACTIVE)
        {
            subHour = 0;
            gameState = GameState.IDLE;
        }
    }

    private void RandomEncounterTick()
    {
        // Ideally this will be an array of possible events for each area

        // Example area
        // 95% nothing happens
        // 1% battles
        // 2% text events
        int randomNum = Random.Range(0, 1000);
        Debug.Log(randomNum);
        if (randomNum < 100)
        {
            // battle against random pool of ships
            // StartBattle()
            if (currentZoneEnemies.Count > 0)
            {
                int randomZoneShip = Random.Range(0, currentZoneEnemies.Count);
                StartBattle(currentZoneEnemies[randomZoneShip]);
            }
        }
        else if (randomNum < 30)
        {
            // Random text event
        }
    }

    private void StartBattle(GameObject enemyShip)
    {
        GameObject newEnemy = Instantiate(enemyShip, enemySpawn);
        enemyShipStats = newEnemy.GetComponent<ShipStats>();

        foreach(ShipWeapon sw in playerShipStats.weapons)
        {
            sw.currentTimeToFire = sw.timeToFire;
        }

        foreach (ShipWeapon sw in enemyShipStats.weapons)
        {
            sw.currentTimeToFire = sw.timeToFire;
        }

        gameState = GameState.BATTLE;
        debugHUD.UpdateHUD(this);
    }

    private void IncrementBattle()
    {
        if (gameState == GameState.BATTLE) {
            if (!playerShipStats.isDead) {
                foreach (ShipWeapon sw in playerShipStats.weapons)
                {
                    sw.currentTimeToFire -= .1f;
                    if (sw.currentTimeToFire <= 0)
                    {
                        StartCoroutine(sw.DoAttack(enemyShipStats));
                        sw.currentTimeToFire = sw.timeToFire;
                    }
                }
            }
            if (!enemyShipStats.isDead) {
                foreach (ShipWeapon sw in enemyShipStats.weapons)
                {
                    sw.currentTimeToFire -= .1f;
                    if (sw.currentTimeToFire <= 0)
                    {
                        StartCoroutine(sw.DoAttack(playerShipStats));
                        sw.currentTimeToFire = sw.timeToFire;
                    }
                }
            }
        }
    }

    public IEnumerator EndBattle(ShipStats loser)
    {
        if (loser.gameObject.tag == "PlayerShip")
        {
            yield return new WaitForSeconds(1f);

            // Game over
            gameState = GameState.ACTIVE;
            DestroyImmediate(enemyShipStats.gameObject, true);
            scrap = scrap / 2;
            playerShipStats.health = playerShipStats.maxHealth;
            consistentHUD.UpdateHUD(this);
            debugHUD.UpdateHUD(this);
            resourceHUD.UpdateHUD(this);
        }
        else
        {
            // Win battle

            // Add rewards
            // (list of reward executable types)

            // Ship anim play death
            yield return new WaitForSeconds(1f);


            DestroyImmediate(loser.gameObject, true);
            gameState = GameState.ACTIVE;
            // Might need to wait for all co routines to end
            debugHUD.UpdateHUD(this);
        }
    }
}
