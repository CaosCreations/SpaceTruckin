using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes
{
    TitleScreen, MainStation, StackMinigame, WheelMinigame, TileMinigame, SimonMinigame
}

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance { get; private set; }

    private static readonly Dictionary<Scenes, string> scenesMapping = new()
    {
        { Scenes.TitleScreen, "TitleScreenScene" },
        { Scenes.MainStation, "ExpandStationScene" },
        { Scenes.StackMinigame, "StackMinigameScene" }
    };

    public static UnityAction<Scenes> OnSceneLoaded;

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

    public void LoadScene(Scenes scene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        var sceneName = GetSceneNameByEnum(scene);

        try
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
            OnSceneLoaded?.Invoke(scene);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to load scene with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    public void LoadSceneAsync(Scenes scene, Slider loadingBarSlider = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        var sceneName = GetSceneNameByEnum(scene);

        try
        {
            StartCoroutine(Instance.LoadAsync(sceneName, loadingBarSlider, loadSceneMode));
            OnSceneLoaded?.Invoke(scene);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to load scene async with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    private IEnumerator LoadAsync(string sceneName, Slider loadingBarSlider = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        Debug.Log("Starting loading scene asynsc...");
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        while (!asyncOperation.isDone)
        {
            var progress = asyncOperation.progress;
            Debug.Log("Loading progress: " + progress);

            if (loadingBarSlider != null)
                loadingBarSlider.value = progress;

            yield return null;
        }
        Debug.Log("Finished loading scene async.");
    }

    public void UnloadSceneAsync(string sceneName)
    {
        try
        {
            StartCoroutine(Instance.UnloadAsync(sceneName));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to unload scene async with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    public void UnloadSceneAsync(Scenes scene)
    {
        var sceneName = GetSceneNameByEnum(scene);
        UnloadSceneAsync(sceneName);
    }

    private IEnumerator UnloadAsync(string sceneName)
    {
        Debug.Log("Starting unloading scene asynsc...");
        var asyncOperation = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            var progress = asyncOperation.progress;
            Debug.Log("Unloading progress: " + progress);
            yield return null;
        }
        Debug.Log("Finished unloading scene async.");
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
