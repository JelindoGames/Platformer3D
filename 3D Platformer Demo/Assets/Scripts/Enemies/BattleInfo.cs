using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfo : MonoBehaviour
{
    public Platform[] platforms;
    public Opponent[] opponents;
}

[System.Serializable]
public class Opponent
{
    public enum Type
    {
        RingShooter,
        PlaceholderType
    }

    public Type type;
    public Vector3 position;
}

[System.Serializable]
public class Platform
{
    public Vector3 position;
    public Vector3 scale;
}
