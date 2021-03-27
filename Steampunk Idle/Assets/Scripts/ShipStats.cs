using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public float evasion;

    public bool isDead = false;

    public List<ShipWeapon> weapons;

    // So we don't need to .find() it
    public GameObject spriteHolder;
    public Transform shipCanvas;

    private void Start()
    {
    }
}
