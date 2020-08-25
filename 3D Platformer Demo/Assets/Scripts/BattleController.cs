using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    public event EventHandler beginPlayerTurn;
    public event EventHandler beginEnemyTurn;
    int liveEnemies;
    int hitEnemies;
    int timeLeftForPlayerTurn;
    int enemyAttacksCompleted = 0;

    public void InitiateBattle() //RN only called by debug key
    {
        TakeHeadCount(this, EventArgs.Empty);
        TransitionToPlayerTurn(); //todo make enemy turn happen under certain conditions
    }

    void Start()
    {
        beginPlayerTurn += TakeHeadCount;
        beginEnemyTurn += TakeHeadCount;
        beginEnemyTurn += KillTimer;
    }

    void TakeHeadCount(object sender, EventArgs e)
    {
        liveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        hitEnemies = 0;
        enemyAttacksCompleted = 0;
    }

    public void SetTimer() //Called by BattleSpawn
    {
        timeLeftForPlayerTurn = 10;
        StartCoroutine("Countdown");
    }

    void KillTimer(object sender, EventArgs e)
    {
        timeLeftForPlayerTurn = -1;
        StopCoroutine("Countdown");
    }

    void EndBattle()
    {
        StopAllCoroutines();
        print("The battle is over!");
    }

    public void ProcessEnemyDeath(GameObject enemyToDestroy) //called by an enemy's damage taker script when killed
    {
        liveEnemies--;

        if (liveEnemies == 0)
        {
            EndBattle();
            return;
        }

        if (hitEnemies == liveEnemies)
        {
            if (timeLeftForPlayerTurn > 0)
                TransitionToEnemyTurn();
        }
    }

    public void ProcessEnemyHit() //Called by an enemy's damage taker script when hit but not killed
    {
        hitEnemies++;

        if (hitEnemies == liveEnemies)
        {
            if (timeLeftForPlayerTurn > 0)
                TransitionToEnemyTurn();
        }
    }

    public void ProcessEnemyAttackEnd() //Called by an enemy's behavior script once its attack has been completed
    {
        enemyAttacksCompleted++;
        liveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length; //Redundancy to fix the bug where Destroy() isn't fast enough to lose an enemy's place in the counter at the beginning of turn

        if (enemyAttacksCompleted == liveEnemies)
        {
            TransitionToPlayerTurn();
        }
    }

    public void TransitionToEnemyTurn() //May be called by BattleFall or Debug Script
    {
        if (liveEnemies > 0)
        {
            beginEnemyTurn?.Invoke(this, EventArgs.Empty);
            print("Transition => Enemy Turn....");
        }
    }

    public void TransitionToPlayerTurn() //May be called by Debug Script
    {
        if (liveEnemies > 0)
        {
            beginPlayerTurn?.Invoke(this, EventArgs.Empty);
            print("Transition => Player Turn....");
        }
    }

    IEnumerator Countdown()
    {
        while (timeLeftForPlayerTurn > 0)
        {
            print(timeLeftForPlayerTurn);
            timeLeftForPlayerTurn--;
            yield return new WaitForSeconds(1);
        }

        TransitionToEnemyTurn();
    }

    public void enablePlayer(int statement, GameObject player, PlayerHorizMovement horizScript, PlayerVertMovement vertScript) //called by battleSpawn after spawn
    {
        horizScript.enabled = true;
        vertScript.enabled = true;

        switch (statement)
        {
            case 1:
                print("End transition. Player attacks!");
                break;
            case 2:
                print("End transition. Enemy attacks!");
                break;
            default:
                break;
        }
    }
}
