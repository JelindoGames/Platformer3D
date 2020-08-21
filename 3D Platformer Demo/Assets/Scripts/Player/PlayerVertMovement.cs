﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVertMovement : MonoBehaviour
{
    [SerializeField] float jumpPower = 0;
    Rigidbody rb;
    bool blastAvailable;
    bool storedOnGround;
    float timeAfterLanding = 0.1f;
    bool leapDiesUponGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator LandingSequence()
    {
        timeAfterLanding = 0;

        while (timeAfterLanding < 0.2f)
        {
            timeAfterLanding += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Blast()
    {
        MetaControl.controlMode = MetaControl.ControlMode.Blast;
        blastAvailable = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.useGravity = false;

        yield return new WaitForSeconds(0.4f);

        if (MetaControl.controlMode == MetaControl.ControlMode.Blast)
        {
            MetaControl.controlMode = MetaControl.ControlMode.PostBlast;
            rb.useGravity = true;
        }

        yield return new WaitForSeconds(0.5f);

        if (MetaControl.controlMode == MetaControl.ControlMode.PostBlast)
            MetaControl.controlMode = MetaControl.ControlMode.Standard;
    }

    IEnumerator Dive()
    {
        MetaControl.controlMode = MetaControl.ControlMode.Dive;
        rb.useGravity = true;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 6, rb.velocity.z);

        yield return new WaitUntil(() => onGround());

        MetaControl.controlMode = MetaControl.ControlMode.PostDive;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift));

        MetaControl.controlMode = MetaControl.ControlMode.DiveRecovery;

        yield return new WaitForSeconds(0.3f);

        MetaControl.controlMode = MetaControl.ControlMode.Standard;
    }

    void Leap()
    {
        leapDiesUponGround = false;
        MetaControl.controlMode = MetaControl.ControlMode.Leap;
        rb.velocity += new Vector3(0, jumpPower / 3, 0); //Adds to the jump velocity that was already there
        StartCoroutine("LeapDieTimer");
    }

    void Update()
    {
        storedOnGround = onGround();

        if (MetaControl.controlMode == MetaControl.ControlMode.Standard || MetaControl.controlMode == MetaControl.ControlMode.Leap)
        {
            blastAvailable = true;
        }

        if (MetaControl.controlMode == MetaControl.ControlMode.Blast)
        {
            if (!onGround() && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine("Dive");
            }
        }

        if (MetaControl.controlMode == MetaControl.ControlMode.Standard || MetaControl.controlMode == MetaControl.ControlMode.Leap || MetaControl.controlMode == MetaControl.ControlMode.PostBlast)
        {
            if (Input.GetKeyDown(KeyCode.Q) && blastAvailable)
            {
                StartCoroutine("Blast");
            }

            if (!onGround() && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine("Dive");
            }

            if (!onGround() && !Input.GetKey(KeyCode.Space))
            {
                rb.velocity -= new Vector3(0, 30 * Time.deltaTime, 0);
            }

            if (Input.GetKeyDown(KeyCode.Space) && onGround() && MetaControl.controlMode != MetaControl.ControlMode.Blast)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);

                if (timeAfterLanding < 0.2f)
                {
                    Leap();
                }
            }

            if (onGround() && MetaControl.controlMode == MetaControl.ControlMode.Leap && leapDiesUponGround)
            {
                MetaControl.controlMode = MetaControl.ControlMode.Standard;
                leapDiesUponGround = false;
            }
        }
    }

    public bool onGround()
    {
        if (Physics.Raycast(transform.position + new Vector3(-0.5f, 0, -0.5f), Vector3.down, 1.05f) || Physics.Raycast(transform.position + new Vector3(0, 0, -0.5f), Vector3.down, 1.05f) || Physics.Raycast(transform.position + new Vector3(0.5f, 0, -0.5f), Vector3.down, 1.05f) || Physics.Raycast(transform.position + new Vector3(-0.5f, 0, 0), Vector3.down, 1.05f) || Physics.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector3.down, 1.05f) || Physics.Raycast(transform.position + new Vector3(-0.5f, 0, 0.5f), Vector3.down, 1.05f) || Physics.Raycast(transform.position + new Vector3(0, 0, 0.5f), Vector3.down, 1.05f) || Physics.Raycast(transform.position + new Vector3(0.5f, 0, 0.5f), Vector3.down, 1.05f))
        {
            if (storedOnGround == false)
            {
                StartCoroutine("LandingSequence");
            }

            return true;
        }

        return false;
    }

    IEnumerator LeapDieTimer()
    {
        yield return new WaitForSeconds(0.5f);
        leapDiesUponGround = true;
    }
}
