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
        StartCoroutine(LeapBack(true));
    }

    void TransitionForEnemyTurn(object sender, EventArgs e)
    {
        StartCoroutine(LeapBack(false));
    }

    IEnumerator LeapBack(bool isForPlayerTurn)
    {
        MetaControl.controlMode = MetaControl.ControlMode.Standard;
        rb.velocity = new Vector3(0, 0, 0);
        horizScript.enabled = false;
        vertScript.enabled = false;
        rb.useGravity = false;
        myCollider.enabled = false;

        Vector3 distanceToMove = -transform.position;
        float distanceMoved = 0;

        while (distanceMoved < distanceToMove.magnitude)
        {
            transform.position += distanceToMove * 5 * Time.deltaTime;
            distanceMoved += distanceToMove.magnitude * 5 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = new Vector3(0, 0, 0); //todo make starting pos more flexible
        myCollider.enabled = true;
        rb.useGravity = true;

        yield return new WaitForSeconds(1);

        horizScript.enabled = true;
        vertScript.enabled = true;

        if (isForPlayerTurn)
        {
            print("The player attacks! You have 10 seconds!");
        }
        else
        {
            print("The enemy attacks!");
        }
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
