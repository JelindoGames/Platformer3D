using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        //transform.LookAt(transform.position - cam.transform.position);
        transform.LookAt(cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
