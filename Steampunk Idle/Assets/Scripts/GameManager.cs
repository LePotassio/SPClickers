using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Gamestate Stuff
    public enum GameState { IDLE, ACTIVE, EVENT, SHOP, BATTLE, PAUSE }
    // ACTIVE no longer exists, remove later
    // Needed new states for other screens... or could have catch all for shop, factory, navigation and collection

    public GameState gs = GameState.IDLE;

    // Resource Tracking Stuff
    private int scrap = 0;
    private int scrapPC = 1;
    private int scrapPS = 0;
    private float scrapButtonCD = 0f;
    private float scrapButtonCDMax = 3f;


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


    // Text Event stuff
    List<TextEvent> textEvents;
    public Button eventButton1;
    public Button eventButton2;
    public TextMeshProUGUI eventTimerText;
    public TextMeshProUGUI eventPromptText;

    // HUD Stuff
    public ResourceHUD resourceHUD;
    public DebugHUD debugHUD;
    public ConsistentHUD consistentHUD;

    public Canvas eventCanvas;

    public Canvas resourceScreen;
    public Canvas debugInfo;

    int eventCDFlag;

    public Animator gearAnim;

    //Hyper laziness intensifies
    public bool[] blueprints;
    public int[] robots;

    //Just for fun
    public List<string> entertainingEvents;
    public int entertainmentBuffer;
    // Actually brings up a good question about two way access... gamemanager needs to see menu handler and vice versa
    public MenuHandlers mh;
    public NaviHandler nh;

    // Travel stuff
    public int timeToShop = 30;

    public GameObject gear;

    public GameObject shopSpritePrefab;
    public GameObject shopSprite;

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

    private void Awake()
    {
        Screen.SetResolution(562, 1000, false);
    }

    private void Start()
    {
        debugHUD.UpdateHUD(this);
        resourceHUD.UpdateHUD(this);
        InvokeRepeating("IncrementTime", 0, 1);
        InvokeRepeating("IncrementBattle", 0, .1f);

        textEvents = new List<TextEvent>();

        foreach (TextEvent te in GameObject.FindObjectsOfType<TextEvent>())
            textEvents.Add(te);

        blueprints = new bool[2];
        for (int i = 0; i < blueprints.Length; i++)
        {
            blueprints[i] = false;
        }

        //0 is golem, 1 is hauler, 2 is scuttler (because changes to hardcode)
        robots = new int[3];

        mh.sbutton.GetComponent<Button>().interactable = false;
        nh.departButton.interactable = false;
    }

    private void IncrementTime()
    {
        if (gameState == GameState.IDLE)
        {

            if (timeToShop == 0)
            {
                gameState = GameState.SHOP;
                mh.sbutton.GetComponent<Button>().interactable = true;
                nh.departButton.interactable = true;
                nh.destTime.text = "Arrived At shop";
                consistentHUD.addStory("Your ship arrives at port. <color=green>Your ship is repaired fully</color>");
                playerShipStats.health = playerShipStats.maxHealth;
                consistentHUD.UpdateHUD(this);
                gear.GetComponent<SpriteRenderer>().enabled = false;
                shopSprite = Instantiate(shopSpritePrefab, enemySpawn);
                //lol this should all be in navi handler or shop maybe
                nh.naviInFlight.SetActive(false);
                nh.naviArrived.SetActive(true);
                return;
            }
            IncrementResources();
            IncrementTimer(passiveTimeIncrement);
            RandomEncounterTick();
        }
        else if (gameState == GameState.ACTIVE) {
            // Events and battles now randomly occur
            IncrementTimerActive(activeTimeIncrement);
            RandomEncounterTick();
        }
        else if (gameState == GameState.BATTLE)
        {
            // This is done in IncrementBattle()
            IncrementResources();
            IncrementTimer(passiveTimeIncrement);
        }
        else if (gameState == GameState.EVENT)
        {
            // Give the player a text prompt and pause resource updates
            IncrementResources();
            IncrementTimer(passiveTimeIncrement);
        }

        if (timeToShop > 0)
        {
            timeToShop--;
            nh.destTime.text = "Time to shop: " + timeToShop;
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
        if (scrapButtonCD == 0)
            gearAnim.SetBool("DoSpin", false);
        resourceHUD.UpdateHUD(this);
    }

    public void OnScrapButton()
    {
        if (scrapButtonCD <= 0)
        {
            gearAnim.SetBool("DoSpin", true);
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
        if (randomNum < 50)
        {
            // battle against random pool of ships
            // StartBattle()
            if (currentZoneEnemies.Count > 0)
            {
                int randomZoneShip = Random.Range(0, currentZoneEnemies.Count);
                StartBattle(currentZoneEnemies[randomZoneShip]);
            }
        }
        else if (randomNum < 100 && eventCDFlag == 0)
        {
            // Random text event
            if (textEvents.Count > 0)
            {
                int randomEvent = Random.Range(0, textEvents.Count);
                StartTextEvent(textEvents[randomEvent]);
            }
        }
        else if (randomNum < 200 && entertainmentBuffer <= 0)
        {
            // Random text event
            if (entertainingEvents.Count > 0)
            {
                int randomEvent = Random.Range(0, entertainingEvents.Count);
                consistentHUD.addStory(entertainingEvents[randomEvent]);
                entertainmentBuffer = 5;
            }
        }

        if (entertainmentBuffer > 0)
            entertainmentBuffer--;
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
        consistentHUD.addStory("As the clouds part you spot a ship in the distance. Pirates! You prepare to battle...");
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
            gameState = GameState.IDLE;
            DestroyImmediate(enemyShipStats.gameObject, false);
            scrap = scrap / 2;

            consistentHUD.addStory("Your ship narrowly escapes and you <color=red>lose half your scrap</color>");

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

            scrap += loser.scrapReward;

            consistentHUD.addStory("You send crew members down to salvage the ship. <color=green>Scrap +" + loser.scrapReward + "</color>");

            resourceHUD.UpdateHUD(this);

            // Ship anim play death
            yield return new WaitForSeconds(1f);


            DestroyImmediate(loser.gameObject, true);
            gameState = GameState.IDLE;
            // Might need to wait for all co routines to end
            debugHUD.UpdateHUD(this);
        }
    }

    public void StartTextEvent(TextEvent textEvent)
    {
        // Add story

        consistentHUD.addStory(textEvent.eventTextShort);
        eventPromptText.text = textEvent.eventText;

        // Open the event text box

        eventCanvas.enabled = true;

        // Assign buttons correct handlers

        eventButton1.onClick.RemoveAllListeners();
        eventButton2.onClick.RemoveAllListeners();

        eventButton1.onClick.AddListener(textEvent.onOption1);
        eventButton1.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = textEvent.option1Name;

        if (textEvent.numOptions == 2)
        {
            eventButton2.enabled = true;
            eventButton2.onClick.AddListener(textEvent.onOption2);
            eventButton2.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = textEvent.option2Name;
        }
        else
        {
            eventButton2.enabled = false;
        }

        gameState = GameState.EVENT;

        eventCDFlag = 30;

        // Start timer
        StartCoroutine(EventTimer(textEvent));
    }

    public IEnumerator EventTimer(TextEvent textEvent)
    {
        int i = 25;

        eventTimerText.text = i.ToString();

        while (i > 0 && eventCanvas.enabled)
        {
            yield return new WaitForSeconds(1f);
            i--;
            eventTimerText.text = i.ToString();

            if (i == 0)
            {
                // consistentHUD.addStory(textEvent.doNothingText);
                textEvent.onDoNothing();
            }
        }

        if (eventCanvas.enabled)
        {
            eventCanvas.enabled = false;
            gameState = GameState.IDLE;
        }
    }

    public IEnumerator EventCDTimer()
    {
        while (eventCDFlag > 0)
        {
            yield return new WaitForSeconds(1f);
            eventCDFlag--;
        }
    }
}
