using System.Collections;
using UnityEngine;

public class SceneLoadFadeIn : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public Color fadeColor = Color.black;

    private Texture2D fadeTexture;
    private float fadeAlpha = 1.0f;

    private void Start()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, fadeColor);
        fadeTexture.Apply();
    } 

    private void OnGUI()
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            fadeAlpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeAlpha = 0.0f;
    }
}
