using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorizMovement : MonoBehaviour
{
    [SerializeField] float speed = 0;
    [SerializeField] float inputSensitivity = 0;
    Rigidbody rb;
    float inputPower;
    float modeMultiplier = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void ModifyInputPower()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
        {
            inputPower += inputSensitivity * Time.deltaTime;
        }
        else
        {
            inputPower -= 2 * inputSensitivity * Time.deltaTime;
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
                modeMultiplier -= 1f * Time.deltaTime;
                break;
            case MetaControl.ControlMode.Standard:
                break;
        }
    }

    void Update()
    {
        ModifyInputPower();
        ModifyModeMultiplier();
    }

    void FixedUpdate()
    {
        if (MetaControl.controlMode == MetaControl.ControlMode.Standard)
        {
            Vector3 horizMovementChange = inputPower * -transform.up * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + horizMovementChange * modeMultiplier);
        }

        if(MetaControl.controlMode == MetaControl.ControlMode.Blast || MetaControl.controlMode == MetaControl.ControlMode.PostBlast)
        {
            Vector3 horizMovementChange = -transform.up * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + horizMovementChange * modeMultiplier);
        }
    }
}
