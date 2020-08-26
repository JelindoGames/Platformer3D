using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawn : MonoBehaviour
{
    BattleController battleController;
    PlayerHorizMovement horizScript;
    PlayerVertMovement vertScript;
    Collider myCollider;
    Rigidbody rb;

    void TransitionForPlayerTurn(object sender, EventArgs e)
    {
        StartCoroutine("LeapBack", 1);
    }

    void TransitionForEnemyTurn(object sender, EventArgs e)
    {
        StartCoroutine("LeapBack", 2);
    }

    IEnumerator LeapBack(int reasonForTransition)
    {
        MetaControl.controlMode = MetaControl.ControlMode.Standard;
        horizScript.enabled = false;
        vertScript.enabled = false;
        rb.useGravity = false;
        myCollider.enabled = false;
        rb.velocity = new Vector3(0, 0, 0);

        Vector3 distanceToMove = -transform.position;
        float distanceMoved = 0;

        while (distanceMoved < distanceToMove.magnitude)
        {
            yield return new WaitForEndOfFrame();
            transform.position += distanceToMove * 5 * Time.deltaTime;
            distanceMoved += distanceToMove.magnitude * 5 * Time.deltaTime;
        }

        transform.position = new Vector3(0, 0, 0); //todo make starting pos more flexible
        myCollider.enabled = true;
        rb.useGravity = true;

        StartCoroutine("FluffPeriod", reasonForTransition);
    }

    IEnumerator FluffPeriod(int reason)
    {
        yield return new WaitForSeconds(1);

        if (reason == 1) battleController.SetTimer();
        battleController.enablePlayer();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>(); //todo: change to whatever collider the player actually is
        horizScript = GetComponent<PlayerHorizMovement>();
        vertScript = GetComponent<PlayerVertMovement>();

        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
        battleController.beginPlayerTurn += TransitionForPlayerTurn;
        battleController.beginEnemyTurn += TransitionForEnemyTurn;
    }
}
