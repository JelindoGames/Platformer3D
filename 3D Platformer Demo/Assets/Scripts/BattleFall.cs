using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFall : MonoBehaviour
{
    BattleController battleController;
    bool isPlayerTurn = true;

    void RespawnPlayer(Collider other)
    {
        if (isPlayerTurn)
        {
            battleController.TransitionToEnemyTurn();
        }
        else
        {
            other.gameObject.GetComponent<BattleSpawn>().StartCoroutine("LeapBack", 0);
        }
    }

    void AcknowledgePlayerTurn(object sender, EventArgs e)
    {
        isPlayerTurn = true;
    }

    void AcknowledgeEnemyTurn(object sender, EventArgs e)
    {
        isPlayerTurn = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<DamageTaker>().TakeDamage(5);
            RespawnPlayer(other);
        }
    }

    void Start()
    {
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
        battleController.beginPlayerTurn += AcknowledgePlayerTurn;
        battleController.beginEnemyTurn += AcknowledgeEnemyTurn;
    }
}
