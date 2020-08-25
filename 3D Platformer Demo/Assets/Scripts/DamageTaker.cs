using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageTaker : MonoBehaviour
{
    //todo display damage a different way
    TextMesh damageText;
    TextMesh healthText;
    Health health;
    BattleController battleController;
    bool isSlowEffectRunning;

    public void TakeDamage(float amount)
    {
        if (!enabled) return;

        health.health -= amount;
        damageText.text = "-" + amount.ToString();

        if (health.health <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                //todo actual game over
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                StartCoroutine("ProcessKill");
            }
        }
        else
        {
            StartCoroutine("ProcessHit");
        }
    }

    IEnumerator ProcessHit()
    {
        StartCoroutine("AnimateText");
        StartCoroutine(SlowEffect(false));

        if (gameObject.CompareTag("Player"))
        {
            StartCoroutine("InvincibilityFrames");
        }
        else
        {
            yield return new WaitUntil(() => !isSlowEffectRunning);
            MetaControl.controlMode = MetaControl.ControlMode.Standard; //todo fallback?
            GetComponent<EnemyHitStatus>()?.BecomeNonTarget(); //If this is the last enemy that can be hit, it will turn into a hazard immediately after this, thanks to the beginEnemyTurn event
            battleController.ProcessEnemyHit();
        }
    }

    IEnumerator ProcessKill()
    {
        StartCoroutine("AnimateText");
        StartCoroutine(SlowEffect(true));
        yield return new WaitUntil(() => !isSlowEffectRunning);
        MetaControl.controlMode = MetaControl.ControlMode.Standard;
        battleController.ProcessEnemyDeath(gameObject);
        Destroy(gameObject);
    }

    IEnumerator AnimateText()
    {
        damageText.color = Color.white;
        healthText.color = Color.white;

        while (damageText.color.g > 0)
        {
            healthText.color -= new Color(3f * Time.deltaTime / Time.timeScale, 3f * Time.deltaTime / Time.timeScale, 3f * Time.deltaTime / Time.timeScale, 0f);
            damageText.color -= new Color(0, 3f * Time.deltaTime / Time.timeScale, 3f * Time.deltaTime / Time.timeScale, 0f);
            yield return new WaitForEndOfFrame();
        }

        while (damageText.color.a > 0)
        {
            damageText.color -= new Color(0, 0, 0, 1f * Time.deltaTime / Time.timeScale);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator InvincibilityFrames()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        gameObject.layer = 8; //will not collide with hazards in this layer

        yield return new WaitForSeconds(2f);

        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        gameObject.layer = 0;
    }

    IEnumerator SlowEffect(bool isKill)
    {
        isSlowEffectRunning = true;

        if (!isKill)
        {
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = 0.001f;
            yield return new WaitForSeconds(0.02f);
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
        else
        {
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = 0.001f;
            yield return new WaitForSeconds(0.06f);
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }

        isSlowEffectRunning = false;
    }

    void Start()
    {
        battleController = GameObject.Find("*BATTLE CONTROLLER*").GetComponent<BattleController>();
        healthText = transform.Find("Health Display").GetComponent<TextMesh>();
        damageText = transform.Find("Damage Display").GetComponent<TextMesh>();
        damageText.text = "";
        health = GetComponent<Health>();
    }
}
