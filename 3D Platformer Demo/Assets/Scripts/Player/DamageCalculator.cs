using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour //More accurately, this class should be called "Decide Player Interaction with Enemies / Hazards"
{
    PlayerHorizMovement horizMovement;
    Rigidbody rb;
    DamageTaker playerDamageTaker;
    BattleController battleController;

    void OnTriggerEnter(Collider other)
    {
        /////////////////DEALING DAMAGE TO AN ENEMY/////////////////////////////////////////////////////////////////////////////////////

        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = other.transform.parent.gameObject;
            while (enemy.GetComponent<IsEnemyCentral>() == null) { enemy = enemy.transform.parent.gameObject; }

            if (GameModeHandler.gamemode == GameModeHandler.GameMode.Battle)
            {
                float damage = GetDamageToEnemy();
                DamageTaker damageTaker = enemy.GetComponent<DamageTaker>();
                damageTaker.TakeDamage(damage);
            }
            else
            {
                BattleEnemySpawnInfo spawnInfo = enemy.GetComponent<BattleEnemySpawnInfo>();
                battleController.InitiateBattle(enemy, spawnInfo);
            }
        }

        /////////////////DEALING DAMAGE TO A PUNCHING BAG/////////////////////////////////////////////////////////////////////////////////////

        if (other.gameObject.CompareTag("PunchingBag"))
        {
            GameObject enemy = other.transform.parent.gameObject;
            while (enemy.GetComponent<DamageTaker>() == null) { enemy = enemy.transform.parent.gameObject; } //todo change how a punching bag takes damage?

            float damage = GetDamageToEnemy();
            DamageTaker damageTaker = enemy.GetComponent<DamageTaker>();
            damageTaker.TakeDamage(damage);
        }

        /////////////////TAKING DAMAGE FROM A HAZARD (TRIGGER) /////////////////////////////////////////////////////////////////////////////////////

        HazardData hazardInfo = other.gameObject.GetComponent<HazardData>(); //GETTING HIT BY A HAZARD

        if (hazardInfo != null && hazardInfo.enabled)
        {
            playerDamageTaker.TakeDamage(hazardInfo.damageToPlayer);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        /////////////////TAKING DAMAGE FROM A HAZARD (COLLISION) /////////////////////////////////////////////////////////////////////////////////////

        HazardData hazardInfo = other.gameObject.GetComponent<HazardData>();

        if (hazardInfo != null && hazardInfo.enabled)
        {
            playerDamageTaker.TakeDamage(hazardInfo.damageToPlayer);
        }
    }

    float GetDamageToEnemy()
    {
        float damage = horizMovement.horizMovementChange.magnitude;
        if (rb.velocity.y < 0) { damage += -rb.velocity.y; }

        damage = (int)damage;
        damage *= 0.1f;

        return damage;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        horizMovement = GetComponent<PlayerHorizMovement>();
        playerDamageTaker = GetComponent<DamageTaker>();
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
    }
}
