using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWizard : MonoBehaviour
{
    enum ControllerType
    {
        PS4
    }

    [SerializeField] ControllerType controllerType;

    void Update()
    {
        switch (controllerType)
        {
            case ControllerType.PS4:
                ControllerWizardData.GetJumpButton = Input.GetKey(KeyCode.Joystick1Button1);
                ControllerWizardData.GetJumpButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button1);
                ControllerWizardData.GetBlastButton = Input.GetKey(KeyCode.Joystick1Button7);
                ControllerWizardData.GetBlastButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button7);
                ControllerWizardData.GetDiveButton = Input.GetKey(KeyCode.Joystick1Button0);
                ControllerWizardData.GetDiveButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button0);
                ControllerWizardData.GetHorizontalCamAxis = Input.GetAxis("PS4_CamHorizontal");
                ControllerWizardData.GetVerticalCamAxis = Input.GetAxis("PS4_CamVertical");
                ControllerWizardData.GetHorizontalMoveAxis = Input.GetAxis("PS4_MoveHorizontal");
                ControllerWizardData.GetVerticalMoveAxis = Input.GetAxis("PS4_MoveVertical");
                break;
        }
    }
}
