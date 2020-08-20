using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_DamageRing : MonoBehaviour
{
    public float speed = 5;

    void Start()
    {
        StartCoroutine("AnimateToLife");
        Invoke("Die", 7);
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, 180 * Time.deltaTime);
        transform.position -= transform.forward * speed * Time.deltaTime;
    }

    IEnumerator AnimateToLife()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        while (transform.localScale.x < 1)
        {
            transform.localScale += new Vector3(5f * Time.deltaTime, 5f * Time.deltaTime, 5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    void Die()
    {
        StartCoroutine("Shrink");
    }

    IEnumerator Shrink()
    {
        while (transform.localScale.x > 0.01f)
        {
            transform.localScale -= new Vector3(2f * Time.deltaTime, 2f * Time.deltaTime, 2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
