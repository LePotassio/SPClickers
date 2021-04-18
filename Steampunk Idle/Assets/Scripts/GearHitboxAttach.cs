using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHitboxAttach : MonoBehaviour
{
    public GameObject gear;
    private void Awake()
    {
        Vector2 gearPos = Camera.main.WorldToScreenPoint(gear.transform.position);
        this.gameObject.transform.position = gearPos;
    }
}
