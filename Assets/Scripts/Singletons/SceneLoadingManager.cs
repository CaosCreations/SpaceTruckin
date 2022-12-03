using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (!scenesMapping.TryGetValue(scene, out var sceneName))
        {
            throw new System.Exception($"Unable to get scene name from enum value: {scene}. It is unmapped.");
        }

        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unable to load scene with name '{sceneName}'.\n{ex.Message}\n{ex.StackTrace}");
        }
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
