using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : MonoBehaviour
{
    public GameObject explosionFireAnimation;
    public GameObject explosionHitAnimation;
    public GameObject hitTextPrefab;

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
        Instantiate(explosionFireAnimation, this.transform);
        yield return new WaitForSeconds(travelTime);

        if (target != null)
        {

            float evasionChance = Random.Range(0, 100);
            // Hit or miss animation here
            if (evasionChance > (100 - accuracy) + target.evasion)
            {
                Instantiate(explosionHitAnimation, target.spriteHolder.transform);
                target.health -= attackDmg;
                TextHitIndicator.CreateTextHit("-" + attackDmg, StaticGlobals.FindChildWithTag(target.transform, "ShipSprite"), hitTextPrefab);
                gm.consistentHUD.UpdateHUD(gm);

                if (target.health <= 0)
                {
                    StartCoroutine(gm.EndBattle(target));
                }
            }
            else
            {
                // Miss animation
                TextHitIndicator.CreateTextHit("Miss", StaticGlobals.FindChildWithTag(target.transform, "ShipSprite"), hitTextPrefab);
            }
        }
    }
}
