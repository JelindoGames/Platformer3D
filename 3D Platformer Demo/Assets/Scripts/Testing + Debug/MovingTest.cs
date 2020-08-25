using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTest : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(Mathf.Sin(Time.time) * 8, 0, 0) * Time.deltaTime;
    }
}
