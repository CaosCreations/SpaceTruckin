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
    }

    private IEnumerator BlinkLoop()
    {
        while (true)
        {
            var interval = Random.Range(currentFaceData.BlinkIntervalMin, currentFaceData.BlinkIntervalMax);
            yield return new WaitForSeconds(interval);

            var blinkSprite = currentFaceData.GetBlinkSprite(currentEyeIndex);
            if (blinkSprite != null)
            {
                eyesImage.sprite = blinkSprite;
                yield return new WaitForSeconds(currentFaceData.BlinkDuration);
                eyesImage.sprite = currentFaceData.Eyes[currentEyeIndex];
            }
        }
    }
}
