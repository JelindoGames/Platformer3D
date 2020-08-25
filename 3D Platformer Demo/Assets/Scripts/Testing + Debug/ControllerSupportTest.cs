using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSupportTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        print(Input.GetAxis("Horizontal"));
    }
}
