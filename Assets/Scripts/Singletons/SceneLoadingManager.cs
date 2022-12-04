using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes
{
    TitleScreen, MainStation
}

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance { get; private set; }

    private static readonly Dictionary<Scenes, string> scenesMapping = new Dictionary<Scenes, string>
    {
        { Scenes.TitleScreen, "TitleScreenScene" },
        { Scenes.MainStation, "ExpandStationScene" }
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(Scenes scene)
    {
        var sceneName = GetSceneNameByEnum(scene);

        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to load scene with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    public void LoadSceneAsync(Scenes scene, Slider loadingBarSlider = null)
    {
        var sceneName = GetSceneNameByEnum(scene);

        try
        {
            StartCoroutine(Instance.LoadAsync(sceneName, loadingBarSlider));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to load scene async with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    private IEnumerator LoadAsync(string sceneName, Slider loadingBarSlider = null)
    {
        Debug.Log("Starting loading scene asynsc...");
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            //var progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            var progress = asyncOperation.progress;
            Debug.Log("Loading progress: " + progress);

            if (loadingBarSlider != null)
                loadingBarSlider.value = progress;

            yield return null;
        }
        Debug.Log("Finished loading scene async.");
    }

    public static string GetSceneNameByEnum(Scenes scene)
    {
        if (!scenesMapping.TryGetValue(scene, out var sceneName))
        {
            throw new System.Exception($"Unable to get scene name from enum value: {scene}. It is unmapped.");
        }
        return sceneName;
    }
}
