using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenBehaviour : MonoBehaviour
{
    public float duration;
    public Image screen;
    public Text text;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        screen = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn() 
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            screen.color = new Color(
                screen.color.r, 
                screen.color.g,
                screen.color.b, 
                Mathf.Lerp(0, 1, currentTime / duration)
            );

            text.color = new Color(
                text.color.r,
                text.color.g,
                text.color.b,
                Mathf.Lerp(0, 1, currentTime / duration)
            );

            yield return null;
        }
    }
}
