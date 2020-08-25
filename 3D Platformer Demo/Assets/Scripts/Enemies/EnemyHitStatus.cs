using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitStatus : MonoBehaviour
{
    [Header("Don't Include Invisible Geometry")]
    [SerializeField] GameObject[] visibleGeometry = null;
    [SerializeField] Material placeholderEnemyMaterial = null;
    BattleController battleController;

    void BecomeTarget(object sender, EventArgs e)
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = true;
            placeholderEnemyMaterial.color = Color.black;
            EnableOpaqueGeometry(true);
        }
    }

    public void BecomeNonTarget() //Called by DamageTaker of enemy when hit
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = false;
            EnableOpaqueGeometry(false);
            //todo actual animation
        }
    }

    void BecomeHazardous(object sender, EventArgs e)
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = false;
            placeholderEnemyMaterial.color = Color.red;
            EnableOpaqueGeometry(true);
        }
    }

    void EnableOpaqueGeometry(bool status)
    {
        if (status == false)
        {
            foreach (GameObject thing in visibleGeometry)
            {
                thing.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
        else
        {
            foreach (GameObject thing in visibleGeometry)
            {
                thing.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }

    void Start()
    {
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
        battleController.beginEnemyTurn += BecomeHazardous;
        battleController.beginPlayerTurn += BecomeTarget;
    }
}
