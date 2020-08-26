using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    PlayerHorizMovement horizMovement;
    Rigidbody rb;
    DamageTaker playerDamageTaker;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("PunchingBag")) //DEALING DAMAGE TO AN ENEMY OR PUNCHING BAG
        {
            if (GameModeHandler.gamemode == GameModeHandler.GameMode.Battle || other.gameObject.CompareTag("PunchingBag"))
            {
                float damage = GetDamageToEnemy();

                GameObject enemy = other.transform.parent.gameObject;
                while (enemy.GetComponent<DamageTaker>() == null) { enemy = enemy.transform.parent.gameObject; }

                DamageTaker damageTaker = enemy.GetComponent<DamageTaker>();
                damageTaker.TakeDamage(damage);
            }
            else
            {
                //todo: Logic that starts the battle
            }
        }

        HazardData hazardInfo = other.gameObject.GetComponent<HazardData>(); //GETTING HIT BY A HAZARD

        if (hazardInfo != null && hazardInfo.enabled)
        {
            playerDamageTaker.TakeDamage(hazardInfo.damageToPlayer);
        }
    }

    void OnCollisionEnter(Collision other) //GETTING HIT BY A HAZARD. (redundancy because some hazards can't be triggers, since they're concave)
    {
        HazardData hazardInfo = other.gameObject.GetComponent<HazardData>();

        if (hazardInfo != null && hazardInfo.enabled)
        {
            playerDamageTaker.TakeDamage(hazardInfo.damageToPlayer);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        horizMovement = GetComponent<PlayerHorizMovement>();
        playerDamageTaker = GetComponent<DamageTaker>();
    }

    float GetDamageToEnemy()
    {
        float damage = horizMovement.horizMovementChange.magnitude;
        if (rb.velocity.y < 0) { damage += -rb.velocity.y; }

        damage = (int)damage;
        damage *= 0.1f;

        return damage;
    }
}
