using Events;
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
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        SingletonManager.EventService.Add<OnSceneLoadedEvent>(OnSceneLoadedHandler);
        SingletonManager.EventService.Add<OnSceneUnloadedEvent>(OnSceneUnloadedHandler);
    }

    public GameObject InitMinigame(RepairsMinigameType minigameType, Transform parent)
    {
        var minigame = minigameContainer.Elements
            .FirstOrDefault(mg => mg != null && mg.RepairsMinigameType == minigameType);

        if (minigame == null)
            throw new Exception("Minigame not found in container with type: " + minigameType);

        Debug.Log("Starting minigame with type: " + minigameType);

        UIManager.ClearCanvases();
        SceneLoadingManager.Instance.LoadSceneAsync(minigame.Scene, loadSceneMode: LoadSceneMode.Additive);

        return default;
    }

    private void SetUpMinigameCamera()
    {
        var camera = GameObject.FindWithTag(RepairsConstants.RepairsMinigameCameraTag);

        if (camera == null)
            throw new Exception("Couldn't find MinigameCamera-tagged object");

        if (!camera.TryGetComponent(out Camera cameraComponent))
            throw new Exception("Couldn't get Camera component");

        cameraComponent.depth = UIConstants.RepairsMinigameCameraDepth;
    }

    private void OnSceneLoadedHandler(OnSceneLoadedEvent loadedEvent)
    {
        if (!loadedEvent.IsRepairsMinigameScene)
            return;

        var minigameBehaviour = FindObjectOfType<RepairsMinigameBehaviour>();
        minigameBehaviour.SetUp();
        SetUpMinigameCamera();
    }

    private void OnSceneUnloadedHandler(OnSceneUnloadedEvent unloadedEvent)
    {
        if (!unloadedEvent.IsRepairsMinigameScene)
            return;

        SceneLoadingManager.Instance.SetActiveScene(SceneType.MainStation);

        // Show hangar canvas when returning back from a repairs minigame 
        UIManager.ShowCanvas(UICanvasType.Hangar);
    }
}