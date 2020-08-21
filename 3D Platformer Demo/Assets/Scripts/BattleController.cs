using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public event EventHandler beginPlayerTurn;
    public event EventHandler beginEnemyTurn;
    int liveEnemies;
    int hitEnemies;
    int timeLeftForPlayerTurn;
    int enemyAttacksCompleted = 0;

    void Start()
    {
        beginPlayerTurn += TakeHeadCount;
        beginPlayerTurn += SetTimer;
        beginEnemyTurn += TakeHeadCount;
        beginEnemyTurn += KillTimer;
    }

    void TakeHeadCount(object sender, EventArgs e)
    {
        liveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        hitEnemies = 0;
        enemyAttacksCompleted = 0;
    }

    void SetTimer(object sender, EventArgs e)
    {
        timeLeftForPlayerTurn = 10;
        StartCoroutine("Countdown");
    }

    void KillTimer(object sender, EventArgs e)
    {
        timeLeftForPlayerTurn = -1;
        StopCoroutine("Countdown");
    }

    public void ProcessEnemyDeath() //called by an enemy's damage taker script when killed
    {
        liveEnemies--;

        if (liveEnemies == 0)
        {
            StopAllCoroutines();
            print("The battle is over!");
            return;
        }

        if (hitEnemies == liveEnemies)
        {
            if (timeLeftForPlayerTurn > 0)
                StartCoroutine("EndPlayerTurn");
        }
    }

    public void ProcessEnemyHit() //called by an enemy's damage taker script when hit but not killed
    {
        hitEnemies++;

        if (hitEnemies == liveEnemies)
        {
            if (timeLeftForPlayerTurn > 0)
                StartCoroutine("EndPlayerTurn");
        }
    }

    public void ProcessEnemyAttackEnd()
    {
        enemyAttacksCompleted++;

        if (enemyAttacksCompleted == liveEnemies)
        {
            StartCoroutine("EndEnemyTurn");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            beginEnemyTurn?.Invoke(this, EventArgs.Empty);
            print("The enemy turn begins....");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            beginPlayerTurn?.Invoke(this, EventArgs.Empty);
            print("The player turn begins....");
        }
    }

    IEnumerator EndPlayerTurn()
    {
        //todo make enemies impossible to hit for this second
        yield return new WaitForSeconds(1);

        beginEnemyTurn?.Invoke(this, EventArgs.Empty);
        print("The enemy turn begins....");
    }

    IEnumerator EndEnemyTurn()
    {
        yield return new WaitForSeconds(1);

        beginPlayerTurn?.Invoke(this, EventArgs.Empty);
        print("The player turn begins....");
    }

    IEnumerator Countdown()
    {
        while (timeLeftForPlayerTurn > 0)
        {
            yield return new WaitForSeconds(1);
            print(timeLeftForPlayerTurn);
            timeLeftForPlayerTurn--;
        }

        StartCoroutine("EndPlayerTurn");
    }
}
