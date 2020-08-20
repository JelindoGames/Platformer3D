using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeHandler : MonoBehaviour
{
    enum GameMode
    {
        PlayerTurn,
        EnemyTurn
    }

    GameMode gamemode;
    public event EventHandler beginPlayerTurn;
    public event EventHandler beginEnemyTurn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            beginEnemyTurn?.Invoke(this, EventArgs.Empty);
            print("The enemy turn begins....");
        }
    }
}
