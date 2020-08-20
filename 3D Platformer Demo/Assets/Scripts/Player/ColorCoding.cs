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
                myMat.color = new Color(1, 0, 0, myMat.color.a);
                break;
            case MetaControl.ControlMode.PostBlast:
                myMat.color = new Color(0.5f, 0, 0, myMat.color.a);
                break;
            case MetaControl.ControlMode.Standard:
                myMat.color = new Color(0.5f, 0.5f, 0.5f, myMat.color.a);
                break;
            case MetaControl.ControlMode.Dive:
                myMat.color = new Color(1f, 1f, 0f, myMat.color.a);
                break;
            case MetaControl.ControlMode.PostDive:
                myMat.color = new Color(0.7f, 0.7f, 0, myMat.color.a);
                break;
            case MetaControl.ControlMode.DiveRecovery:
                myMat.color = new Color(0.7f, 0.7f, 0.4f, myMat.color.a);
                break;
            case MetaControl.ControlMode.Leap:
                myMat.color = new Color(0, 1f, 0, myMat.color.a);
                break;

        }
    }
}
