using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    PlayerHorizMovement horizMovement;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        horizMovement = GetComponent<PlayerHorizMovement>();
    }

    float GetDamage()
    {
        float damage = horizMovement.horizMovementChange.magnitude;
        if (rb.velocity.y < 0) { damage += -rb.velocity.y; }

        damage = (int)damage;
        damage *= 0.1f;

        return damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            float damage = GetDamage();
            GameObject enemy = other.transform.parent.gameObject;
            DamageTaker damageTaker = enemy.GetComponent<DamageTaker>();
            damageTaker.TakeDamage(damage);
            StartCoroutine("SlowEffect");
        }
    }

    IEnumerator SlowEffect()
    {
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = 0.001f;
        yield return new WaitForSeconds(0.005f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }
}
