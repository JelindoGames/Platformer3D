using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorizMovement : MonoBehaviour
{
    [SerializeField] float speed = 0;
    [SerializeField] float inputSensitivity = 0;
    PlayerVertMovement verticalMovement;
    Rigidbody rb;
    float inputPower;
    float modeMultiplier = 1;
    bool getMeaningfulInput;
    bool getMeaningfulControllerInput;
    [HideInInspector] public Vector3 horizMovementChange;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        verticalMovement = GetComponent<PlayerVertMovement>();
    }

    void ModifyInputPower()
    {
        if (getMeaningfulInput)
        {
            if (verticalMovement.onGround()) { inputPower += inputSensitivity * Time.deltaTime; }
            else { inputPower += inputSensitivity * Time.deltaTime * 0.75f; }
        }
        else if (getMeaningfulControllerInput)
        {
            float controllerPower = new Vector2(ControllerWizardData.GetHorizontalMoveAxis, ControllerWizardData.GetVerticalMoveAxis).magnitude;
            if (verticalMovement.onGround()) { inputPower += inputSensitivity * controllerPower * Time.deltaTime; }
            else { inputPower += inputSensitivity * controllerPower * Time.deltaTime * 0.75f; }
        }
        else
        {
            if (verticalMovement.onGround()) { inputPower -= 2 * inputSensitivity * Time.deltaTime; }
            else { inputPower -= inputSensitivity * Time.deltaTime * 0.5f; }
        }

        inputPower = Mathf.Clamp(inputPower, 0f, 1f);
    }

    void ModifyModeMultiplier()
    {
        switch (MetaControl.controlMode)
        {
            case MetaControl.ControlMode.Blast:
                modeMultiplier = 2f;
                break;
            case MetaControl.ControlMode.PostBlast:
                modeMultiplier -= 2.5f * Time.deltaTime;
                break;
            case MetaControl.ControlMode.Standard:
                modeMultiplier = 1;
                break;
            case MetaControl.ControlMode.Dive:
                modeMultiplier = 1.8f;
                break;
            case MetaControl.ControlMode.PostDive:
                if (modeMultiplier > 0.1f) { modeMultiplier -= 4f * Time.deltaTime; }
                else { modeMultiplier = 0f; }
                break;
            case MetaControl.ControlMode.DiveRecovery:
                if (modeMultiplier < 1.4f && getMeaningfulInput) { modeMultiplier += 5f * Time.deltaTime; }
                else if (modeMultiplier >= 1.4f) { modeMultiplier = 1.4f; }
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { transform.position = Vector3.zero; }
        getMeaningfulInput = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.D));
        getMeaningfulControllerInput = Mathf.Abs(ControllerWizardData.GetVerticalMoveAxis) > Mathf.Epsilon || Mathf.Abs(ControllerWizardData.GetHorizontalMoveAxis) > Mathf.Epsilon;
        ModifyInputPower();
        ModifyModeMultiplier();
    }

    void FixedUpdate()
    {
        if (MetaControl.controlMode == MetaControl.ControlMode.Standard || MetaControl.controlMode == MetaControl.ControlMode.Leap)
        {
            horizMovementChange = inputPower * -transform.up * speed * Time.fixedDeltaTime * modeMultiplier;
            rb.velocity = new Vector3(horizMovementChange.x, rb.velocity.y, horizMovementChange.z);
        }

        if(MetaControl.controlMode == MetaControl.ControlMode.Blast || MetaControl.controlMode == MetaControl.ControlMode.PostBlast)
        {
            horizMovementChange = -transform.up * speed * Time.fixedDeltaTime * modeMultiplier;
            rb.velocity = new Vector3(horizMovementChange.x, rb.velocity.y, horizMovementChange.z);
        }

        if (MetaControl.controlMode == MetaControl.ControlMode.Dive || MetaControl.controlMode == MetaControl.ControlMode.PostDive || MetaControl.controlMode == MetaControl.ControlMode.DiveRecovery)
        {
            horizMovementChange = -transform.up * speed * Time.fixedDeltaTime * modeMultiplier;
            rb.velocity = new Vector3(horizMovementChange.x, rb.velocity.y, horizMovementChange.z);
        }
    }
}
