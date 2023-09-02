using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private Image image;
    private bool isAnimating;

    public void StartAnimation()
    {
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(AnimateImage());
        }
    }

    public void StopAnimation()
    {
        isAnimating = false;
        image.sprite = sprites[0];
    }

    private IEnumerator AnimateImage()
    {
        int currentFrame = 0;
        while (isAnimating)
        {
            image.sprite = sprites[currentFrame];
            yield return new WaitForSeconds(frameRate);
            currentFrame = (currentFrame + 1) % sprites.Length;
        }
    }
}
