using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    //todo display damage a different way
    TextMesh damageText;
    TextMesh healthText;
    Health health;

    void Start()
    {
        healthText = transform.Find("Health Display").GetComponent<TextMesh>();
        damageText = transform.Find("Damage Display").GetComponent<TextMesh>();
        damageText.text = "";
        health = GetComponent<Health>();
    }

    public void TakeDamage(float amount)
    {
        health.health -= amount;
        damageText.text = "-" + amount.ToString();
        StartCoroutine("AnimateText");
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
}
