using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaControl : MonoBehaviour
{
    public enum ControlMode
    {
        Standard,
        Blast,
        PostBlast,
        Dive
    }

    public static ControlMode controlMode;
}
