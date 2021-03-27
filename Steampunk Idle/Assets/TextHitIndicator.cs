using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TextHitIndicator : MonoBehaviour
{
    public TextMeshPro hitText;

    public static TextHitIndicator CreateTextHit(string newHitText, Transform parentCanvas, GameObject hitTextPrefab)
    {
        GameObject newTHGM = Instantiate(hitTextPrefab, parentCanvas) as GameObject;
        // newTHGM.transform.position = location.position;
        TextHitIndicator newTHI = newTHGM.GetComponent<TextHitIndicator>();
        newTHI.hitText.text = newHitText;
        return newTHI;
    }
}
