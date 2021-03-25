using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : MonoBehaviour
{
    public int attackDmg;
    public float currentTimeToFire;
    public float timeToFire;
    public float travelTime;
    public float accuracy;
    // public GameObject weaponSprite; // Will be component of the weapon game object
    public GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    public IEnumerator DoAttack(ShipStats target)
    {
        // Fire animation here
        yield return new WaitForSeconds(travelTime);

        if (target != null)
        {

            float evasionChance = Random.Range(0, 100);
            // Hit or miss animation here
            if (evasionChance > (100 - accuracy) + target.evasion)
            {
                target.health -= attackDmg;
                gm.consistentHUD.UpdateHUD(gm);

                if (target.health <= 0)
                {
                    gm.EndBattle(target);
                }
            }
        }
    }
}
