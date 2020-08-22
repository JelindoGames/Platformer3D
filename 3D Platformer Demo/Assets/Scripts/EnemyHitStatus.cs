using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitStatus : MonoBehaviour
{
    [SerializeField] Collider[] colliders = null;
    [SerializeField] Material placeholderEnemyMaterial = null;
    BattleController battleController;

    void BecomeHazardous(object sender, EventArgs e)
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            placeholderEnemyMaterial.color = Color.red;
        }
    }

    void BecomeTarget(object sender, EventArgs e)
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = true;
            transform.localScale = new Vector3(1f, 1f, 1f);
            placeholderEnemyMaterial.color = Color.black;
        }
    }

    public void BecomeNonTarget()
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = false;
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //todo actual animation
        }
    }

    void Start()
    {
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
        battleController.beginEnemyTurn += BecomeHazardous;
        battleController.beginPlayerTurn += BecomeTarget;
    }
}
