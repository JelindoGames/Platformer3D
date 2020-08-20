using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_RingShooter : MonoBehaviour
{
    GameModeHandler gameModeHandler;
    [SerializeField] GameObject hazardRing = null;

    IEnumerator Attack()
    {
        for (int i = 0; i < 15; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
            GameObject ringInstance = Instantiate(hazardRing, spawnPosition, Quaternion.Euler(0, GetAngleToPlayer(), 0));
            ringInstance.transform.parent = transform;
            ringInstance.name = "Ring";

            yield return new WaitForSeconds(0.3f);
        }
    }

    void Start()
    {
        gameModeHandler = GameObject.Find("*GAME MODE HANDLER*").GetComponent<GameModeHandler>();
        gameModeHandler.beginEnemyTurn += StartTurn;
    }

    void StartTurn(object sender, EventArgs e)
    {
        StartCoroutine("Attack");
    }

    float GetAngleToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody playerRB = player.GetComponent<Rigidbody>();

        Vector2 xzDistanceToPlayer = new Vector2(transform.position.x - player.transform.position.x, transform.position.z - player.transform.position.z);
        float angleToPlayer = Mathf.Atan2(xzDistanceToPlayer.y, xzDistanceToPlayer.x) * Mathf.Rad2Deg;
        angleToPlayer = -angleToPlayer + 90; //conversion to actually get it facing in right direction
        angleToPlayer += UnityEngine.Random.Range(-10 - (2 * playerRB.velocity.magnitude), 10 + (2 * playerRB.velocity.magnitude));
        return angleToPlayer;
    }
}
