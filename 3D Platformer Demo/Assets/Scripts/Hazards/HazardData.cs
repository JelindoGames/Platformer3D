using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardData : MonoBehaviour
{
    public float damageToPlayer;

    void Start()
    {
        if (gameObject.layer != 9)
        {
            Debug.LogError("This hazard is not on the appropriate layer. Was this intentional?");
        }
    }
}
