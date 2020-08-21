using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    //todo display damage a different way
    TextMesh damageText;
    TextMesh healthText;
    Health health;
    BattleController battleController;

    IEnumerator SlowEffect()
    {
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = 0.001f;
        yield return new WaitForSeconds(0.005f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    public void TakeDamage(float amount)
    {
        health.health -= amount;
        damageText.text = "-" + amount.ToString();

        if (health.health <= 0)
        {
            if (!gameObject.CompareTag("Player"))
            {
                battleController.ProcessEnemyDeath();
                Destroy(gameObject); //todo Do a death animation
            }
        }
        else
        {
            StartCoroutine("AnimateText");
            StartCoroutine("SlowEffect");

            if (gameObject.CompareTag("Player"))
            {
                StartCoroutine("InvincibilityFrames");
            }
            else
            {
                battleController.ProcessEnemyHit();
                //todo make enemy unhittable here
            }
        }
    }

    IEnumerator AnimateText()
    {
        damageText.color = Color.white;
        healthText.color = Color.white;

        while (damageText.color.g > 0)
        {
            healthText.color -= new Color(1f * Time.deltaTime, 1f * Time.deltaTime, 1f * Time.deltaTime, 0f);
            damageText.color -= new Color(0, 1f * Time.deltaTime, 1f * Time.deltaTime, 0f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator InvincibilityFrames()
    {
        Material myMaterial = GetComponent<Renderer>().material;
        myMaterial.color = new Color(myMaterial.color.r, myMaterial.color.g, myMaterial.color.b, 0.5f);
        gameObject.layer = 8; //will not collide with hazards in this layer

        yield return new WaitForSeconds(2f);

        myMaterial.color = new Color(myMaterial.color.r, myMaterial.color.g, myMaterial.color.b, 1f);
        gameObject.layer = 0;
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
