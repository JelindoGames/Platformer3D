using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTest : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, Mathf.Sin(Time.time) * 8) * Time.deltaTime;
    }
}
