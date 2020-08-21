using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeHandler : MonoBehaviour
{
    public enum GameMode
    {
        Battle,
        Overworld
    }

    public static GameMode gamemode;
}
