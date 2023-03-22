using System.Collections;
using UnityEngine;

public class ElementWrapAround : MonoBehaviour
{
    [SerializeField]
    private float speed = 100.0f;

    private bool isWrappingEnabled = false;

    [SerializeField]
    private bool start = false;

    private RectTransform rectTransform;
    private float objectWidth;
    private float screenWidth;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        objectWidth = rectTransform.rect.width;
        screenWidth = Screen.width;
    }

    private void Update()
    {
        if (start)
        {
            StartCoroutine(StartWrapAround());
            start = false;
        }

        if (!isWrappingEnabled)
            return;

        // Move the parent GameObject to the right until the duplicate object is off screen
        if (rectTransform.anchoredPosition.x < screenWidth + objectWidth / 2)
        {
            rectTransform.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);
        }
        else
        {
            // Set the position of the parent GameObject to the left of the screen
            rectTransform.anchoredPosition = new Vector2(-objectWidth / 2, 0);
        }
    }

    public IEnumerator StartWrapAround()
    {
        isWrappingEnabled = true;
        while (isWrappingEnabled && rectTransform.anchoredPosition.x > -screenWidth / 2)
        {
            yield return null;
        }
        // Stop wrapping when the object comes back to its original position
        StopWrapAround();
    }

    public void StopWrapAround()
    {
        isWrappingEnabled = false;

        // Set the position of the parent GameObject to the original position
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
