using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("The center position is where the platform spawns.")]
    public Vector3 positionChangeFromCenter;
    public float period;
    Vector3 cycleCenter;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cycleCenter = transform.position; //Must instantiate in the same line as the position is defined
    }

    void FixedUpdate()
    {
        if (period > 0)
        {
            float timeInCycle = Time.time / period;

            transform.position = new Vector3
            (
                cycleCenter.x + (positionChangeFromCenter.x * Mathf.Sin(timeInCycle)),
                cycleCenter.y + (positionChangeFromCenter.y * Mathf.Sin(timeInCycle)),
                cycleCenter.z + (positionChangeFromCenter.z * Mathf.Sin(timeInCycle))
            );
        }
    }
}
