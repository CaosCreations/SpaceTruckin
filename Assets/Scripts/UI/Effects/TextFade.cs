using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private float timeCoeff = 1f;

    [SerializeField]
    private float duration = 1.5f;

    [SerializeField]
    private bool startFadeIn;

    [SerializeField]
    private bool startFadeOut;

    [SerializeField]
    private bool startFadeInAndOut;

    public void FadeOutText()
    {
        StartCoroutine(Fade(0f));
    }

    private void FadeInText()
    {
        StartCoroutine(Fade(1f));
    }

    private IEnumerator Fade(float targetAlpha, Action action = null)
    {
        targetAlpha = Mathf.Clamp(targetAlpha, 0f, 1f);
        var elapsedTime = 0f;
        var startingColor = text.color;

        // The colour we want to end up with
        var targetColor = new Color(startingColor.r, startingColor.g, startingColor.b, targetAlpha);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * timeCoeff;
            text.color = Color.Lerp(startingColor, targetColor, elapsedTime);
            yield return null;
        }

        action?.Invoke();
    }

    public void SetTextWithFade(string textContent)
    {
        // If text is already there, fade it out first 
        if (!string.IsNullOrWhiteSpace(text.text) && text.color.a >= 1)
        {
            StartCoroutine(Fade(0f, () =>
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
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        text.SetText(textContent);
        FadeInText();
    }

    public void Clear()
    {
        text.Clear();
    }

    public void SetText(string textContent)
    {
        text.SetText(textContent);
    }

    private void OnValidate()
    {
        timeCoeff = Mathf.Max(1f, timeCoeff);
    }

    private void Start()
    {
        timeCoeff = Mathf.Max(1f, timeCoeff);
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
