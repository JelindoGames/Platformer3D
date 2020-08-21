using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    PlayerHorizMovement horizMovement;
    Rigidbody rb;
    DamageTaker playerDamageTaker;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (GameModeHandler.gamemode == GameModeHandler.GameMode.Battle)
            {
                float damage = GetDamageToEnemy();
                GameObject enemy = other.transform.parent.gameObject;
                DamageTaker damageTaker = enemy.GetComponent<DamageTaker>();
                damageTaker.TakeDamage(damage);
            }
            else
            {
                //todo: Logic that starts the battle
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            HazardData hazardData = other.gameObject.GetComponent<HazardData>();

            if (hazardData == null)
            {
                Debug.LogError("The hazard doesn't have the HazardData script attached.");
            }
            else
            {
                playerDamageTaker.TakeDamage(hazardData.damageToPlayer);
            }
        }
    }
}
