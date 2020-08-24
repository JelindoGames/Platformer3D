using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWizard : MonoBehaviour
{
    enum ControllerType
    {
        PS4,
        Xbox //only works on Windows. Unconfirmed to actually work on 360 OR one
    }

    [SerializeField] ControllerType controllerType = ControllerType.PS4;
    float blastPower;

    void Update()
    {
        float storedBlastPower = blastPower;
        blastPower = Input.GetAxis("XB_BlastButton");


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
                ControllerWizardData.GetSnapLeftButton = Input.GetKey(KeyCode.Joystick1Button4);
                ControllerWizardData.GetSnapLeftButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button4);
                ControllerWizardData.GetSnapRightButton = Input.GetKey(KeyCode.Joystick1Button5);
                ControllerWizardData.GetSnapRightButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button5);
                break;
            case ControllerType.Xbox: //only works on Windows. Unconfirmed to actually work on 360 OR one
                ControllerWizardData.GetJumpButton = Input.GetKey(KeyCode.Joystick1Button0);
                ControllerWizardData.GetJumpButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button0);
                ControllerWizardData.GetBlastButton = blastPower > 0;
                ControllerWizardData.GetBlastButtonDown = blastPower > 0 && storedBlastPower <= 0;
                ControllerWizardData.GetDiveButton = Input.GetKey(KeyCode.Joystick1Button2);
                ControllerWizardData.GetDiveButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button2);
                ControllerWizardData.GetHorizontalCamAxis = Input.GetAxis("XB_CamHorizontal");
                ControllerWizardData.GetVerticalCamAxis = Input.GetAxis("XB_CamVertical");
                ControllerWizardData.GetHorizontalMoveAxis = Input.GetAxis("PS4_MoveHorizontal");
                ControllerWizardData.GetVerticalMoveAxis = Input.GetAxis("PS4_MoveVertical");
                ControllerWizardData.GetSnapLeftButton = Input.GetKey(KeyCode.Joystick1Button4);
                ControllerWizardData.GetSnapLeftButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button4);
                ControllerWizardData.GetSnapRightButton = Input.GetKey(KeyCode.Joystick1Button5);
                ControllerWizardData.GetSnapRightButtonDown = Input.GetKeyDown(KeyCode.Joystick1Button5);
                break;
        }
    }
}
