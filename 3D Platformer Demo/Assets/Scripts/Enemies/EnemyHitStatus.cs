using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitStatus : MonoBehaviour
{
    [SerializeField] GameObject[] visibleGeometry = null;
    [SerializeField] GameObject[] hittableGeometry = null;
    [SerializeField] Material placeholderEnemyMaterial = null;
    BattleController battleController;

    void BecomeTarget(object sender, EventArgs e)
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = true;
            enableHazardProperties(false);
            placeholderEnemyMaterial.color = Color.black;
            EnableOpaqueGeometry(true);
        }
    }

    public void BecomeNonTarget() //Called by DamageTaker of enemy when hit
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = false;
            enableHazardProperties(false);
            EnableOpaqueGeometry(false);
            //todo actual animation
        }
    }

    void BecomeHazardous(object sender, EventArgs e)
    {
        if (this != null)
        {
            GetComponent<DamageTaker>().enabled = false;
            enableHazardProperties(true);
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

    void enableHazardProperties(bool status)
    {
        if (status == false)
        {
            foreach (GameObject thing in hittableGeometry)
            {
                HazardData hazardData = thing.GetComponent<HazardData>();
                thing.layer = 0;

                if (hazardData == null) { Debug.LogError("No Hazard Data attached to some hittable geometry"); }
                else { thing.GetComponent<HazardData>().enabled = false; }
            }
        }
        else
        {
            foreach (GameObject thing in hittableGeometry)
            {
                HazardData hazardData = thing.GetComponent<HazardData>();
                thing.layer = 9;

                if (hazardData == null) { Debug.LogError("No Hazard Data attached to some hittable geometry"); }
                else { thing.GetComponent<HazardData>().enabled = true; }
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
