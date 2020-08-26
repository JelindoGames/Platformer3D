using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControls : MonoBehaviour
{
    BattleController battleController;

    void Start()
    {
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            battleController.InitiateBattle(null);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            battleController.TransitionToEnemyTurn();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            battleController.TransitionToPlayerTurn();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
