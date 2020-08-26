using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    [SerializeField] BattleFadeIn battleInitiationImage = null;
    [SerializeField] PlayerHorizMovement horizScript = null;
    [SerializeField] PlayerVertMovement vertScript = null;
    [SerializeField] GameObject player = null;
    [SerializeField] Rigidbody playerRB = null;
    [SerializeField] GameObject enemyHolder = null;

    public event EventHandler beginPlayerTurn;
    public event EventHandler beginEnemyTurn;
    int liveEnemies;
    int hitEnemies;
    int enemyAttacksCompleted = 0;
    [HideInInspector] public int timeLeftForPlayerTurn; //for access by the Battle Timer Display

    Vector3 savedPosition;

    void Start()
    {
        timeLeftForPlayerTurn = -1;
        beginPlayerTurn += TakeHeadCount;
        beginEnemyTurn += TakeHeadCount;
        beginEnemyTurn += KillTimer;
    }

    void TakeHeadCount(object sender, EventArgs e)
    {
        liveEnemies = enemyHolder.transform.childCount; //todo only count the enemies under the EnemySpawner object
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
        }
    }

    public void TransitionToPlayerTurn() //May be called by Debug Script
    {
        if (liveEnemies > 0)
        {
            beginPlayerTurn?.Invoke(this, EventArgs.Empty);
        }
    }

    IEnumerator Countdown()
    {
        while (timeLeftForPlayerTurn > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeftForPlayerTurn--;
        }

        TransitionToEnemyTurn();
    }

    public void enablePlayer() //called by battleSpawn after Spawn Fluff Period, and this script after battle end
    {
        horizScript.enabled = true;
        vertScript.enabled = true;
    }

    public void InitiateBattle(GameObject enemyBeingFought) //Called by Debug Key or Hitting an Enemy in the Overworld
    {
        GameModeHandler.gamemode = GameModeHandler.GameMode.Battle;
        StartCoroutine("BattleStartSequence", enemyBeingFought);
    }

    void EndBattle()
    {
        GameModeHandler.gamemode = GameModeHandler.GameMode.Overworld;
        StopAllCoroutines();
        StartCoroutine("BattleEndSequence");
    }

    IEnumerator BattleStartSequence(GameObject enemyFromOverworld)
    {
        savedPosition = player.transform.position;
        horizScript.enabled = false;
        vertScript.enabled = false;
        playerRB.velocity = Vector3.zero;
        playerRB.useGravity = false;
        battleInitiationImage.Transition();

        yield return new WaitUntil(() => battleInitiationImage.fadedIn);

        battleInitiationImage.fadedIn = false; //for next battle
        Destroy(enemyFromOverworld);
        MetaControl.controlMode = MetaControl.ControlMode.Standard;
        player.transform.position = Vector3.zero;
        playerRB.useGravity = true;

        yield return new WaitUntil(() => battleInitiationImage.fadedOut);
        battleInitiationImage.fadedOut = false; //for next battle
        yield return new WaitForSeconds(1);

        TakeHeadCount(this, EventArgs.Empty);
        TransitionToPlayerTurn(); //todo make enemy turn happen under certain conditions
    }

    IEnumerator BattleEndSequence()
    {
        horizScript.enabled = false;
        vertScript.enabled = false;
        playerRB.velocity = Vector3.zero;
        battleInitiationImage.Transition();

        yield return new WaitUntil(() => battleInitiationImage.fadedIn);

        battleInitiationImage.fadedIn = false; //for next battle
        player.transform.position = savedPosition;

        yield return new WaitUntil(() => battleInitiationImage.fadedOut);
        battleInitiationImage.fadedOut = false; //for next battle

        enablePlayer();
    }
}
