using System.Collections;
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
    bool getMaintainedJumpInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator LandingSequence()
    {
        timeAfterLanding = 0;

        while (timeAfterLanding < 0.1f)
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

        if (MetaControl.controlMode == MetaControl.ControlMode.Dive) { MetaControl.controlMode = MetaControl.ControlMode.PostDive; }
        else yield break;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || ControllerWizardData.GetJumpButtonDown || Input.GetKeyDown(KeyCode.LeftShift) || ControllerWizardData.GetDiveButtonDown);

        if (MetaControl.controlMode == MetaControl.ControlMode.PostDive) { MetaControl.controlMode = MetaControl.ControlMode.DiveRecovery; }
        else yield break;

        yield return new WaitForSeconds(0.3f);

        MetaControl.controlMode = MetaControl.ControlMode.Standard;
    }

    void Leap()
    {
        MetaControl.controlMode = MetaControl.ControlMode.Leap;
        leapDiesUponGround = false;
        StartCoroutine("LeapDieTimer");
        rb.velocity += new Vector3(0, jumpPower / 3, 0); //Adds to the jump velocity that was already there
    }

    void Update()
    {
        storedOnGround = onGround();

        if (Input.GetKeyDown(KeyCode.Space) || ControllerWizardData.GetJumpButtonDown)
            StartCoroutine("MaintainJumpInput");

        if (MetaControl.controlMode == MetaControl.ControlMode.Standard)
            StopCoroutine("Dive");

        if (MetaControl.controlMode == MetaControl.ControlMode.Standard || MetaControl.controlMode == MetaControl.ControlMode.Leap)
        {
            blastAvailable = true;
        }

        if (MetaControl.controlMode == MetaControl.ControlMode.Blast)
        {
            if (!onGround() && (Input.GetKeyDown(KeyCode.LeftShift) || ControllerWizardData.GetDiveButtonDown))
            {
                StartCoroutine("Dive");
            }
        }

        if (MetaControl.controlMode == MetaControl.ControlMode.Standard || MetaControl.controlMode == MetaControl.ControlMode.Leap || MetaControl.controlMode == MetaControl.ControlMode.PostBlast)
        {
            if ((Input.GetKeyDown(KeyCode.Q) || ControllerWizardData.GetBlastButtonDown) && blastAvailable)
            {
                StartCoroutine("Blast");
            }

            if (!onGround() && (Input.GetKeyDown(KeyCode.LeftShift) || ControllerWizardData.GetDiveButtonDown))
            {
                StartCoroutine("Dive");
            }

            if (!onGround() && !Input.GetKey(KeyCode.Space) && !ControllerWizardData.GetJumpButton)
            {
                rb.velocity -= new Vector3(0, 30 * Time.deltaTime, 0);
            }

            if (getMaintainedJumpInput && onGround() && MetaControl.controlMode != MetaControl.ControlMode.Blast)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);

                if (timeAfterLanding < 0.1f)
                {
                    Leap();
                }
            }

            if (MetaControl.controlMode == MetaControl.ControlMode.Leap && onGround() && leapDiesUponGround)
            {
                MetaControl.controlMode = MetaControl.ControlMode.Standard;
                leapDiesUponGround = false;
            }
        }
    }

    public bool onGround()
    {
        Vector3[] positionsToStartRay = { new Vector3(-0.5f, 0, -0.5f), new Vector3(0, 0, -0.5f), new Vector3(0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(-0.5f, 0, 0.5f), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 0.5f) };

        for (int i = 0; i < positionsToStartRay.Length; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + positionsToStartRay[i], Vector3.down, out hit, 1.05f))
            {
                if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Hazard"))
                {
                    if (storedOnGround == false)
                    {
                        StartCoroutine("LandingSequence");
                    }

                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator MaintainJumpInput()
    {
        getMaintainedJumpInput = true;

        float timer = 0;

        while (timer < 0.05f && (Input.GetKey(KeyCode.Space) || ControllerWizardData.GetJumpButton))
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        getMaintainedJumpInput = false;
    }

    IEnumerator LeapDieTimer()
    {
        yield return new WaitForSeconds(0.5f);
        leapDiesUponGround = true;
    }
}
