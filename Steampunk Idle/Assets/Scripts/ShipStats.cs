using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public float evasion;

    public List<ShipWeapon> weapons;

    private void Start()
    {
    }
}
