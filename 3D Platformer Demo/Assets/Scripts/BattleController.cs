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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void CallEndPlayerTurn()
    {
        StopCoroutine("EndPlayerTurn");
        StartCoroutine("EndPlayerTurn");
    }

    IEnumerator EndPlayerTurn()
    {
        //todo make enemies impossible to hit for this second
        //yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(0);

        if (liveEnemies > 0)
        {
            beginEnemyTurn?.Invoke(this, EventArgs.Empty);
            print("The enemy turn begins....");
        }
    }

    IEnumerator EndEnemyTurn()
    {
        yield return new WaitForSeconds(1);

        if (liveEnemies > 0)
        {
            beginPlayerTurn?.Invoke(this, EventArgs.Empty);
            print("The player turn begins....");
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1); //To coincide with the respawn second

        while (timeLeftForPlayerTurn > 0)
        {
            print(timeLeftForPlayerTurn);
            timeLeftForPlayerTurn--;
            yield return new WaitForSeconds(1);
        }

        StartCoroutine("EndPlayerTurn");
    }
}
