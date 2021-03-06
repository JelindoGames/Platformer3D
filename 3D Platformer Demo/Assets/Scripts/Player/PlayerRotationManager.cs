﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationManager : MonoBehaviour
{
    [SerializeField] BasicCamControls camController = null;
    [SerializeField] PlayerVertMovement vertMovement = null;
    float turnSmoothVelocity;
    float turnTime;

    float GetAngleChangeByInput()
    {
        if (Mathf.Abs(ControllerWizardData.GetVerticalMoveAxis) > Mathf.Epsilon || Mathf.Abs(ControllerWizardData.GetHorizontalMoveAxis) > Mathf.Epsilon)
        {
            float y = ControllerWizardData.GetVerticalMoveAxis;
            float x = ControllerWizardData.GetHorizontalMoveAxis;
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            angle = -angle + 90;
            if (angle > 180) angle -= 360;
            return angle;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            return -45;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            return 45;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            return -135;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            return 135;
        }

        if (Input.GetKey(KeyCode.D))
        {
            return 90;
        }

        if (Input.GetKey(KeyCode.A))
        {
            return -90;
        }

        if (Input.GetKey(KeyCode.S))
        {
            return 180;
        }

        return 0;
    }

    float GetGoalAngle()
    {
        float camAngle = (camController.angle * Mathf.Rad2Deg);
        float convertedCamAngle = 270 - camAngle; //version of CamAngle that can be used for calculations HERE

        convertedCamAngle += GetAngleChangeByInput();

        while (!(convertedCamAngle < 180 && convertedCamAngle >= -180)) //Force the convertedAngle into minimum transitionable range (-180 <= c < 180) [myAngle is minimum 0]
        {
            if (convertedCamAngle < -180) { convertedCamAngle += 360; }
            else if (convertedCamAngle >= 180) { convertedCamAngle -= 360; }
        }

        float myAngle = transform.localEulerAngles.y;
        bool isDifferenceAppropriate = Mathf.Abs(convertedCamAngle - myAngle) <= 180;

        while (!isDifferenceAppropriate)
        {
            convertedCamAngle += 360;
            isDifferenceAppropriate = Mathf.Abs(convertedCamAngle - myAngle) <= 180;
        }

        return convertedCamAngle;
    }

    void DecideTurnTime ()
    {
        switch (MetaControl.controlMode)
        {
            case MetaControl.ControlMode.Standard:
                if (vertMovement.onGround()) { turnTime = 0.04f; } else { turnTime = 0.3f; }
                break;
            case MetaControl.ControlMode.Blast:
                turnTime = 0.4f;
                break;
            case MetaControl.ControlMode.PostBlast:
                if (vertMovement.onGround()) { turnTime = 0.15f; } else { turnTime = 0.5f; }
                break;
            case MetaControl.ControlMode.Dive:
                turnTime = 1;
                break;
            case MetaControl.ControlMode.PostDive:
                turnTime = 1;
                break;
            case MetaControl.ControlMode.DiveRecovery:
                turnTime = 0.3f;
                break;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Mathf.Abs(ControllerWizardData.GetVerticalMoveAxis) > Mathf.Epsilon || Mathf.Abs(ControllerWizardData.GetHorizontalMoveAxis) > Mathf.Epsilon)
        {
            float myAngle = transform.localEulerAngles.y;
            float goalAngle = GetGoalAngle();
            DecideTurnTime();
            float transitionAngle = Mathf.SmoothDampAngle(myAngle, goalAngle, ref turnSmoothVelocity, turnTime);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transitionAngle, transform.localEulerAngles.z);
        }
    }
}
