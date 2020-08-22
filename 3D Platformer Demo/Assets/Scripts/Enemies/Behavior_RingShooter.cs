using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_RingShooter : MonoBehaviour
{
    BattleController battleController;
    [SerializeField] GameObject hazardRing = null;
    [SerializeField] float numberOfRings = 5;
    [SerializeField] float timeBetweenRings = 1;
    [SerializeField] float heightOfRings = 1;

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < numberOfRings; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, heightOfRings, 0);
            GameObject ringInstance = Instantiate(hazardRing, spawnPosition, Quaternion.Euler(0, GetAngleToPlayer(), 0));
            ringInstance.transform.parent = transform;
            ringInstance.name = "Ring";

            yield return new WaitForSeconds(timeBetweenRings);
        }

        yield return new WaitForSeconds(2);
        battleController.ProcessEnemyAttackEnd();
    }

    void Start()
    {
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
        battleController.beginEnemyTurn += StartTurn;
    }

    void StartTurn(object sender, EventArgs e)
    {
        if (this != null)
        {
            StartCoroutine("Attack");
        }
    }

    float GetAngleToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody playerRB = player.GetComponent<Rigidbody>();

        Vector2 xzDistanceToPlayer = new Vector2(transform.position.x - player.transform.position.x, transform.position.z - player.transform.position.z);
        float angleToPlayer = Mathf.Atan2(xzDistanceToPlayer.y, xzDistanceToPlayer.x) * Mathf.Rad2Deg;
        angleToPlayer = -angleToPlayer + 90; //conversion to actually get it facing in right direction
        float xzPlayerVelocity = new Vector2(playerRB.velocity.x, playerRB.velocity.z).magnitude;
        angleToPlayer += UnityEngine.Random.Range(-10 - (3 * xzPlayerVelocity), 10 + (3 * xzPlayerVelocity));
        return angleToPlayer;
    }
}
