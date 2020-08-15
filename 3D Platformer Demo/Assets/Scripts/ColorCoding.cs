using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCoding : MonoBehaviour
{
    Material myMat;

    void Start()
    {
        myMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        switch (MetaControl.controlMode)
        {
            case MetaControl.ControlMode.Blast:
                myMat.color = Color.red;
                break;
            case MetaControl.ControlMode.PostBlast:
                myMat.color = new Color(0.5f, 0, 0);
                break;
            case MetaControl.ControlMode.Standard:
                myMat.color = Color.gray;
                break;
            case MetaControl.ControlMode.Dive:
                myMat.color = Color.yellow;
                break;
            case MetaControl.ControlMode.PostDive:
                myMat.color = new Color(0.7f, 0.7f, 0);
                break;
            case MetaControl.ControlMode.DiveRecovery:
                myMat.color = new Color(0.7f, 0.7f, 0.4f);
                break;
        }
    }
}
