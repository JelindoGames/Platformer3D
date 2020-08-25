using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    PlayerHorizMovement horizMovement;
    Rigidbody rb;
    DamageTaker playerDamageTaker;

    void OnCollisionEnter(Collision other) //GETTING HIT BY A HAZARD
    {
        HazardData hazardInfo = other.gameObject.GetComponent<HazardData>();

        if (hazardInfo != null && hazardInfo.enabled)
        {
            playerDamageTaker.TakeDamage(hazardInfo.damageToPlayer);
        }
    }

    void OnTriggerEnter(Collider other) //DEALING DAMAGE TO AN ENEMY
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (GameModeHandler.gamemode == GameModeHandler.GameMode.Battle)
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
