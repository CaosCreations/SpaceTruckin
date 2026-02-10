using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CompositePortraitRenderer : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image baseImage;
    [SerializeField] private Image mouthImage;
    [SerializeField] private Image eyesImage;

    private Coroutine blinkCoroutine;
    private ActorFaceData currentFaceData;
    private int currentEyeIndex;

    public void SetExpression(ActorFaceData faceData, int eyeIndex, int mouthIndex)
    {
        if (faceData.Eyes == null || eyeIndex < 0 || eyeIndex >= faceData.Eyes.Length)
        {
            Debug.LogError($"[CompositePortrait] Invalid eye index {eyeIndex} for actor '{faceData.name}' (Eyes length: {faceData.Eyes?.Length ?? 0})");
            return;
        }

        if (faceData.Mouths == null || mouthIndex < 0 || mouthIndex >= faceData.Mouths.Length)
        {
            Debug.LogError($"[CompositePortrait] Invalid mouth index {mouthIndex} for actor '{faceData.name}' (Mouths length: {faceData.Mouths?.Length ?? 0})");
            return;
        }

        baseImage.sprite = faceData.BaseSprite;
        mouthImage.sprite = faceData.Mouths[mouthIndex];
        eyesImage.sprite = faceData.Eyes[eyeIndex];

        currentFaceData = faceData;
        currentEyeIndex = eyeIndex;

        StopBlinking();
        StartBlinking();
    }

    public void Show()
    {
        root.SetActive(true);
    }

    public void Hide()
    {
        StopBlinking();
        root.SetActive(false);
    }

    private void StartBlinking()
    {
        blinkCoroutine = StartCoroutine(BlinkLoop());
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (currentFaceData != null && currentFaceData.Eyes != null
            && currentEyeIndex >= 0 && currentEyeIndex < currentFaceData.Eyes.Length)
        {
            eyesImage.sprite = currentFaceData.Eyes[currentEyeIndex];
        }
    }

    private void OnDisable()
    {
        StopBlinking();
    }

    private IEnumerator BlinkLoop()
    {
        while (true)
        {
            var interval = Random.Range(currentFaceData.BlinkIntervalMin, currentFaceData.BlinkIntervalMax);
            yield return new WaitForSecondsRealtime(interval);

            var blinkSprite = currentFaceData.GetBlinkSprite(currentEyeIndex);
            if (blinkSprite != null)
            {
                eyesImage.sprite = blinkSprite;
                yield return new WaitForSecondsRealtime(currentFaceData.BlinkDuration);
                eyesImage.sprite = currentFaceData.Eyes[currentEyeIndex];
            }
        }
    }
}
