using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //todo change default health based on enemy type
    public float health;

    //todo make health display in a different way
    TextMesh healthText;

    void Start()
    {
        healthText = transform.Find("Health Display").GetComponent<TextMesh>();
    }

    void Update()
    {
        //Keep the health with one decimal place.
        health = Mathf.Round(health * 10) / 10;
        healthText.text = health.ToString();
    }
}
