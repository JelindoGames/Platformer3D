﻿using System.Collections;
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
                modeMultiplier = 2;
                break;
            case MetaControl.ControlMode.PostBlast:
                modeMultiplier -= 1f * Time.deltaTime;
                break;
            case MetaControl.ControlMode.Standard:
                modeMultiplier = 1;
                break;
            case MetaControl.ControlMode.Dive:
                modeMultiplier = 2f;
                break;
            case MetaControl.ControlMode.PostDive:
                if (modeMultiplier > 0.1f) { modeMultiplier -= 2f * Time.deltaTime; }
                else { modeMultiplier = 0f; }
                break;
            case MetaControl.ControlMode.DiveRecovery:
                if (modeMultiplier < 1.4f && getMeaningfulInput) { modeMultiplier += 3f * Time.deltaTime; }
                else if (modeMultiplier >= 1.4f) { modeMultiplier = 1.4f; }
                break;
        }
    }

    void Update()
    {
        getMeaningfulInput = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.D));
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

        if (MetaControl.controlMode == MetaControl.ControlMode.Dive || MetaControl.controlMode == MetaControl.ControlMode.PostDive || MetaControl.controlMode == MetaControl.ControlMode.DiveRecovery)
        {
            Vector3 horizMovementChange = -transform.up * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + horizMovementChange * modeMultiplier);
        }
    }
}
