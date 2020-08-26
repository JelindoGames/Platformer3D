using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamControls : MonoBehaviour
{
    [SerializeField] Transform subject = null;
    [SerializeField] float mouseSensitivity = 0;
    [SerializeField] float vertMouseSensitivity = 0;
    [SerializeField] float controllerSensitivity = 0;
    [SerializeField] float vertControllerSensitivity = 0;
    [HideInInspector] public float angle = Mathf.PI * 3f/2f;
    [SerializeField] float snapSpeed = 1;
    float offsetDistance;
    float y = 0;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 initialOffset = transform.position - subject.transform.position;
        Vector3 initialOffsetXZ = initialOffset - new Vector3(0, initialOffset.y, 0);
        offsetDistance = initialOffsetXZ.magnitude;
    }

    void ModifyAngle()
    {
        angle -= Input.GetAxis("Mouse X") * mouseSensitivity;
        angle -= ControllerWizardData.GetHorizontalCamAxis * controllerSensitivity * Time.deltaTime;
        if (GameModeHandler.gamemode == GameModeHandler.GameMode.Overworld)
                    angle -= ControllerWizardData.GetHorizontalMoveAxis * controllerSensitivity * 0.2f * Time.deltaTime;
    }

    void SnapAngle()
    {
        if (ControllerWizardData.GetSnapLeftButtonDown)
        {
            StartCoroutine("SnapAngleAnimation", 90);
        }

        if (ControllerWizardData.GetSnapRightButtonDown)
        {
            StartCoroutine("SnapAngleAnimation", -90);
        }
    }

    Vector3 GetOffset()
    {
        float x = Mathf.Cos(angle) * offsetDistance;
        float z = Mathf.Sin(angle) * offsetDistance;
        y += Input.GetAxis("Mouse Y") * vertMouseSensitivity;
        y += ControllerWizardData.GetVerticalCamAxis * vertControllerSensitivity * Time.deltaTime;
        y = Mathf.Clamp(y, -1, 3);
        Vector3 offset = new Vector3(x, 3 + y, z);
        return offset;
    }

    void Update()
    {
        ModifyAngle();
        SnapAngle();
    }

    void LateUpdate()
    {
        Vector3 offset = GetOffset();
        transform.position = subject.transform.position + offset;
        Vector3 subjectXZ = subject.transform.position - new Vector3(0, subject.transform.position.y, 0);
        transform.LookAt(subjectXZ + new Vector3(0, subject.transform.position.y, 0));
    }

    IEnumerator SnapAngleAnimation(float degrees)
    {
        degrees = degrees * Mathf.Deg2Rad;

        float amountMoved = 0;
        float timeSinceStart = 0;

        while (amountMoved < 90)
        {
            timeSinceStart += Time.deltaTime;
            float prevAmountMoved = amountMoved;
            amountMoved = Mathf.SmoothStep(0, degrees, timeSinceStart * snapSpeed);
            angle += (amountMoved - prevAmountMoved);
            yield return new WaitForEndOfFrame();
        }
    }
}
