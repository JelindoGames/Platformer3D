using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour //More accurately, this class should be called "Decide Player Interaction with Enemies / Hazards"
{
    PlayerHorizMovement horizMovement;
    Rigidbody rb;
    DamageTaker playerDamageTaker;
    BattleController battleController;
    Vector3 storedPosition;
    Vector3 absoluteMovement;

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
                BattleInfo battleInfo = enemy.GetComponent<BattleInfo>();
                battleController.InitiateBattle(enemy, battleInfo);
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
        Vector2 horizAbsoluteMovement = new Vector2(absoluteMovement.x, absoluteMovement.z);
        float horizontalContribution = horizAbsoluteMovement.magnitude * 5;
        horizontalContribution = Mathf.Pow(horizontalContribution, 1.4f);

        float verticalContribution = 0;

        if (absoluteMovement.y < 0) { verticalContribution = -absoluteMovement.y * 5; }
        verticalContribution = Mathf.Pow(verticalContribution, 1.4f);

        float damage = (horizontalContribution + verticalContribution) * 10f;
        damage = (int)damage;
        damage *= 0.1f;

        print("Damage: " + damage + "\n" +
            "Horizontal Contribution: " + horizontalContribution + " | " +
            "Vertical Contribution: " + verticalContribution);

        return damage;
    }

    void FixedUpdate()
    {
        GetAbsoluteMovement();
    }

    void GetAbsoluteMovement()
    {
        absoluteMovement = transform.position - storedPosition;
        storedPosition = transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        horizMovement = GetComponent<PlayerHorizMovement>();
        playerDamageTaker = GetComponent<DamageTaker>();
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
        storedPosition = transform.position;
    }
}
