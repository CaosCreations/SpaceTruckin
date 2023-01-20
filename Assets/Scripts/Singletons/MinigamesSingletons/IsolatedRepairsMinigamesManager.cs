using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsolatedRepairsMinigamesManager : MonoBehaviour, IRepairsMinigamesManager
{
    public static IsolatedRepairsMinigamesManager Instance { get; private set; }

    [SerializeField]
    private RepairsMinigameContainer minigameContainer;

    private RepairsMinigameBehaviour[] repairsMinigameBehaviours;

    private readonly Scenes[] repairsMinigameScenes = new[]
    {
        Scenes.StackMinigame, Scenes.TileMinigame, Scenes.WheelMinigame, Scenes.SimonMinigame
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

        repairsMinigameBehaviours = FindObjectsOfType<RepairsMinigameBehaviour>();

        if (repairsMinigameBehaviours == null)
            Debug.LogError("No repairs minigames found");
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoadedHandler;
    }

    public GameObject InitMinigame(RepairsMinigameType minigameType, Transform parent)
    {
        var minigame = minigameContainer.Elements
            .FirstOrDefault(mg => mg != null && mg.RepairsMinigameType == minigameType);

        if (minigame == null)
            throw new Exception("Minigame not found in container with type: " + minigameType);

        Debug.Log("Starting minigame with type: " + minigameType);

        SceneLoadingManager.Instance.LoadSceneAsync(minigame.Scene, loadSceneMode: LoadSceneMode.Additive);
        UIManager.ClearCanvases();
        return default;
    }

    private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "StackMinigameScene")
            return;

        var camera = GameObject.FindWithTag("MinigameCamera");

        if (camera == null)
            throw new Exception("Couldn't find MinigameCamera-tagged object");

        if (!camera.TryGetComponent(out Camera cam))
            throw new Exception("Couldn't get Camera component");

        cam.depth = UIConstants.RepairsMinigameCameraDepth;
    }
}