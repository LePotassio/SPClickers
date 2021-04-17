using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MenuHandlers : MonoBehaviour
{
    public GameManager gm;

    //public List<Canvas> canvasList;

    public Canvas consistentCanvas;
    public Canvas resourceCanvas;


    public Canvas shopCanvas;

    public Canvas collectionCanvas;

    public Canvas factoryCanvas;

    public Canvas naviCanvas;

    public GameObject cam;
    Vector3 initCamPos;

    // For setting colors...
    public GameObject mbutton;
    public GameObject fbutton;
    public GameObject sbutton;
    public GameObject nbutton;
    public GameObject cbutton;

    private void Update()
    {
        // If not at shop, disable shop button...
    }

    private void Start()
    {
        initCamPos = cam.transform.position;

        disableAllCanvas();
        resourceCanvas.transform.Find("ResourceHUD").Find("ScrapPSText").gameObject.SetActive(true);
        consistentCanvas.GetComponent<Canvas>().gameObject.SetActive(true);
    }

    public void onMain()
    {
        // Main resource page
        disableAllCanvas();
        resourceCanvas.transform.Find("ResourceHUD").Find("ScrapPSText").gameObject.SetActive(true);
        consistentCanvas.GetComponent<Canvas>().gameObject.SetActive(true);

        // To avoid pain, we will just move the camera...
        cam.transform.position = initCamPos;

        decolorButtons();
        // I actually have no idea why this isnt working, may need to toggle for it to work...
        //mbutton.GetComponent<Image>().color = new Color(110, 250, 160, 1);
        mbutton.GetComponent<Image>().color = Color.green;
    }

    public void onCollection()
    {
        // Colletion of all purchases
        disableAllCanvas();
        cam.transform.position = new Vector3(-500, 0, -10);
        collectionCanvas.enabled = true;

        decolorButtons();
        cbutton.GetComponent<Image>().color = Color.green;
    }

    public void onNavi()
    {
        // Just a 2 state with either at shop, or in progress with a button enabled when at a shop
        disableAllCanvas();
        cam.transform.position = new Vector3(-1000, 0, -10);
        naviCanvas.enabled = true;

        decolorButtons();
        nbutton.GetComponent<Image>().color = Color.green;
    }

    public void onFactory()
    {
        // List of blueprints with make button
        disableAllCanvas();
        cam.transform.position = new Vector3(-1500, 0, -10);
        factoryCanvas.enabled = true;

        decolorButtons();
        fbutton.GetComponent<Image>().color = Color.green;
    }

    public void onShop()
    {
        // List of the two blueprints...
        disableAllCanvas();
        cam.transform.position = new Vector3(-2000, 0, -10);
        shopCanvas.enabled = true;

        decolorButtons();
        sbutton.GetComponent<Image>().color = Color.green;
    }

    public void disableAllCanvas ()
    {
        //Remember to not include the resource canvas as we want scrap count on all pages

        //Disable all...

        resourceCanvas.transform.Find("ResourceHUD").Find("ScrapPSText").gameObject.SetActive(false);
        consistentCanvas.GetComponent<Canvas>().gameObject.SetActive(false);

        collectionCanvas.enabled = false;
        naviCanvas.enabled = false;
        factoryCanvas.enabled = false;
        shopCanvas.enabled = false;
    }

    public void disableChildren(GameObject gm)
    {
        foreach(Transform child in gm.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void enableChildren(GameObject gm)
    {
        foreach (Transform child in gm.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void decolorButtons()
    {
        mbutton.GetComponent<Image>().color = Color.white;

        fbutton.GetComponent<Image>().color = Color.white;

        sbutton.GetComponent<Image>().color = Color.white;

        nbutton.GetComponent<Image>().color = Color.white;

        cbutton.GetComponent<Image>().color = Color.white;
    }
}
