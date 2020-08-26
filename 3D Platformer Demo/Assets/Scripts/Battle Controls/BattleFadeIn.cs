using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleFadeIn : MonoBehaviour
{
    public bool fadedIn = false;
    public bool fadedOut = false;
    Image image;

    public void Transition()
    {
        StartCoroutine("FadeInAndOut");
    }

    IEnumerator FadeInAndOut()
    {
        while (image.color.a < 1f)
        {
            image.color += new Color(0, 0, 0, 1f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        fadedIn = true;

        while (image.color.a > 0f)
        {
            image.color -= new Color(0, 0, 0, 1f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        fadedOut = true;
    }

    void Start()
    {
        image = GetComponent<Image>();
    }
}
