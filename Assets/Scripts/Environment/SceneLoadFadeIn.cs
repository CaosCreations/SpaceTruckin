using System.Collections;
using UnityEngine;

public class SceneLoadFadeIn : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public Color fadeColor = Color.black;
    public float noiseIntensity = 0.2f;
    public float noiseSpeed = 1.0f;

    private Texture2D fadeTexture;
    private float fadeAlpha = 1.0f;

    [SerializeField]
    private bool manualStart;

    private void OnGUI()
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }

    private void Update()
    {
        if (manualStart)
        {
            StartFadeIn();
            manualStart = false;
        }
    }

    public void StartFadeIn()
    {
        fadeTexture = CreateProceduralTexture();
        StartCoroutine(FadeIn());
    }

    private Texture2D CreateProceduralTexture()
    {
        Texture2D texture = new(Screen.width, Screen.height);
        Color[] pixels = new Color[Screen.width * Screen.height];

        for (int x = 0; x < Screen.width; x++)
        {
            for (int y = 0; y < Screen.height; y++)
            {
                float noiseX = Mathf.PerlinNoise((float)x / Screen.width * noiseSpeed, (float)y / Screen.height * noiseSpeed);
                float noiseY = Mathf.PerlinNoise((float)y / Screen.height * noiseSpeed, (float)x / Screen.width * noiseSpeed);
                float noise = (noiseX + noiseY) * 0.5f * noiseIntensity;
                Color color = fadeColor + new Color(noise, noise, noise, 0f);
                pixels[x + y * Screen.width] = color;
            }
        }

#pragma warning disable UNT0017 // SetPixels invocation is slow
        texture.SetPixels(pixels);
#pragma warning restore UNT0017 // SetPixels invocation is slow
        texture.Apply();
        return texture;
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
