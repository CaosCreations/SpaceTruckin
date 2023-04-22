using Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    TitleScreen, MainStation, Credits, StackMinigame, WheelMinigame, TileMinigame, SimonMinigame
}

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance { get; private set; }

    private static readonly Dictionary<SceneType, string> sceneMappings = new()
    {
        { SceneType.TitleScreen, "TitleScreenScene" },
        { SceneType.MainStation, "ExpandStationScene" },
        { SceneType.Credits, "CreditsScene" },
        { SceneType.StackMinigame, "StackMinigameScene" },
        { SceneType.TileMinigame, "TileMinigameScene" }
    };

    private readonly HashSet<string> loadedSceneNames = new();

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

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        SceneManager.sceneLoaded += SceneLoadedHandler;
        SceneManager.sceneUnloaded += SceneUnloadedHandler;
        SingletonManager.EventService.Add<OnUITransitionEndedEvent>(OnEndOfCalendarTransitionHandler);
    }

    private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SingletonManager.Instance == null)
            return;

        loadedSceneNames.Add(scene.name);
        SingletonManager.EventService.Dispatch(new OnSceneLoadedEvent(scene));
    }

    private void SceneUnloadedHandler(Scene scene)
    {
        if (SingletonManager.Instance == null)
            return;

        loadedSceneNames.Remove(scene.name);
        SingletonManager.EventService.Dispatch(new OnSceneUnloadedEvent(scene));
    }

    public void LoadScene(SceneType sceneType, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        var sceneName = GetSceneNameByType(sceneType);
        try
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to load scene with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    public void LoadSceneAsync(
        SceneType sceneType,
        Slider loadingBarSlider = null,
        LoadSceneMode loadSceneMode = LoadSceneMode.Single,
        bool setActiveScene = false)
    {
        var sceneName = GetSceneNameByType(sceneType);

        // Don't load scene if already loaded 
        if (IsSceneLoaded(sceneName))
        {
            Debug.Log("Scene is already loaded: " + sceneName);
            return;
        }

        try
        {
            StartCoroutine(Instance.LoadAsync(sceneName, loadingBarSlider, loadSceneMode, setActiveScene));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to load scene async with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    private IEnumerator LoadAsync(
        string sceneName,
        Slider loadingBarSlider = null,
        LoadSceneMode loadSceneMode = LoadSceneMode.Single,
        bool setActiveScene = false)
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

        if (setActiveScene)
            SetActiveScene(sceneName);
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

    public void UnloadSceneAsync(SceneType scene)
    {
        var sceneName = GetSceneNameByType(scene);

        if (!IsSceneLoaded(sceneName))
        {
            Debug.Log($"Scene '{sceneName}' is not loaded. Unable to unload.");
            return;
        }

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

    public void SetActiveScene(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }

    public void SetActiveScene(SceneType sceneType)
    {
        var sceneName = GetSceneNameByType(sceneType);
        SetActiveScene(sceneName);
    }

    public static string GetSceneNameByType(SceneType scene)
    {
        if (!sceneMappings.TryGetValue(scene, out var sceneName))
        {
            throw new System.Exception($"Unable to get scene name from enum value: {scene}. It is unmapped.");
        }
        return sceneName;
    }

    public static SceneType GetSceneTypeByName(string sceneName)
    {
        var mapping = sceneMappings.FirstOrDefault(kvp => kvp.Value == sceneName);
        return mapping.Key;
    }

    public static SceneType GetCurrentSceneType()
    {
        var scene = SceneManager.GetActiveScene();
        return GetSceneTypeByName(scene.name);
    }

    public static Scene[] GetLoadedScenes()
    {
        var countLoaded = SceneManager.sceneCount;
        var loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }
        return loadedScenes;
    }

    public static bool IsSceneLoaded(string sceneName)
    {
        var loadedScenes = GetLoadedScenes();
        return loadedScenes.Any(scene => scene.name == sceneName);
    }

    public static bool IsLoadedSceneName(string sceneName)
    {
        return Instance.loadedSceneNames.Contains(sceneName);
    }

    public static bool IsLoadedSceneName(SceneType sceneType)
    {
        var sceneName = GetSceneNameByType(sceneType);
        return Instance.loadedSceneNames.Contains(sceneName);
    }

    private void OnEndOfCalendarTransitionHandler(OnUITransitionEndedEvent evt)
    {
        if (evt.TransitionType != TransitionUI.TransitionType.EndOfCalendar)
            return;

        LoadSceneAsync(SceneType.Credits);
    }
}
