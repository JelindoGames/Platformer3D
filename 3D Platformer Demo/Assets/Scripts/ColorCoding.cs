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
                myMat.color = Color.green;
                break;
            case MetaControl.ControlMode.Standard:
                myMat.color = Color.gray;
                break;
        }
    }
}
