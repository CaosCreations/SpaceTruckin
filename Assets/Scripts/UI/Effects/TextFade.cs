using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private float fadeSpeed;

    [SerializeField]
    private bool startFadeIn;

    [SerializeField]
    private bool startFadeOut;

    public void FadeOutText()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut(Action action = null)
    {
        while (text.color.a > 0)
        {
            var color = text.color;
            color.a -= Time.deltaTime * fadeSpeed;
            text.color = color;
            yield return null;
        }

        action?.Invoke();
    }

    private void FadeInText()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn(Action action = null)
    {
        while (text.color.a < 1)
        {
            var color = text.color;
            color.a += Time.deltaTime * fadeSpeed;
            text.color = color;
            yield return null;
        }

        action?.Invoke();
    }

    public void SetTextWithFade(string textContent)
    {
        text.SetText(textContent);

    }

    private void OnValidate()
    {
        fadeSpeed = Mathf.Max(0f, fadeSpeed);
    }

    private void Update()
    {
        if (startFadeIn)
        {
            FadeInText();
            startFadeIn = false;
        }

        if (startFadeOut)
        {
            FadeOutText();
            startFadeOut = false;
        }
    }
}
