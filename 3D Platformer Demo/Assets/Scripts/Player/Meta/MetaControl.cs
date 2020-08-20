using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaControl : MonoBehaviour
{
    public enum ControlMode
    {
        Standard,
        Leap,
        Blast,
        PostBlast,
        Dive,
        PostDive,
        DiveRecovery
    }

    public static ControlMode controlMode;
}
