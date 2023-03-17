using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private bool start;

    [SerializeField]
    private AnimationCurve animationCurve;

    private void Update()
    {
        if (start)
        {
            StartCoroutine(Shake());
            start = false;
        }
    }

    private IEnumerator Shake()
    {
        var startPosition = transform.position;
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var strength = animationCurve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}