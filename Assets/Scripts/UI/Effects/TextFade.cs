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

    [SerializeField]
    private bool startFadeInAndOut;

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
        // If text is already there, fade it out first 
        if (text.text.Length > 0 && text.color.a >= 1)
        {
            StartCoroutine(FadeOut(() =>
            {
                SetTextAndFadeIn(textContent);
            }));
        }
        else
        {
            SetTextAndFadeIn(textContent);
        }
    }

    private void SetTextAndFadeIn(string textContent)
    {
        text.SetText(textContent);
        FadeInText();
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

        if (startFadeInAndOut)
        {
            SetTextWithFade("New text here");
            startFadeInAndOut = false;
        }
    }
}
