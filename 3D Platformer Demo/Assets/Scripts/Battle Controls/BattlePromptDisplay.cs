using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePromptDisplay : MonoBehaviour
{
    BattleController battleController;
    [SerializeField] Text[] textsToDisplayOn = null;

    void Start()
    {
        foreach (Text txt in textsToDisplayOn) { txt.text = ""; }
        battleController = GetComponent<BattleController>();
        battleController.beginEnemyTurn += initiateEnemyTurnText;
        battleController.beginPlayerTurn += initiatePlayerTurnText;
    }

    void Update()
    {
        if (GameModeHandler.gamemode == GameModeHandler.GameMode.Overworld)
        {
            foreach (Text txt in textsToDisplayOn) { txt.text = ""; }
        }
    }

    IEnumerator EnemyTurnSequence()
    {
        foreach (Text txt in textsToDisplayOn) { txt.text = "READY..."; }
        yield return new WaitForSeconds(1);
        foreach (Text txt in textsToDisplayOn) { txt.text = "AVOID!"; }
    }

    IEnumerator PlayerTurnSequence()
    {
        foreach (Text txt in textsToDisplayOn) { txt.text = "READY..."; }
        yield return new WaitForSeconds(1);
        foreach (Text txt in textsToDisplayOn) { txt.text = "ATTACK!"; }
    }

    void initiateEnemyTurnText (object sender, EventArgs e)
    {
        StartCoroutine("EnemyTurnSequence");
    }

    void initiatePlayerTurnText(object sender, EventArgs e)
    {
        StartCoroutine("PlayerTurnSequence");
    }
}
