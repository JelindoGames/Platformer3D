using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTimerDisplay : MonoBehaviour
{
    BattleController battleController;
    [SerializeField] Text[] textsToDisplayOn = null;

    void Start()
    {
        battleController = GetComponent<BattleController>();
    }

    void Update()
    {
        foreach(Text txt in textsToDisplayOn)
        {
            float timeLeft = battleController.timeLeftForPlayerTurn;

            if (timeLeft >= 0) { txt.text = timeLeft.ToString(); }
            else { txt.text = ""; }
        }
    }
}
