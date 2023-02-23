using Events;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRepairsMinigamesManager : MonoBehaviour, IRepairsMinigamesManager
{
    public static SceneRepairsMinigamesManager Instance { get; private set; }

    [SerializeField]
    private RepairsMinigameContainer minigameContainer;

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
        SingletonManager.EventService.Add<OnSceneLoadedEvent>(OnSceneLoadedHandler);
        SingletonManager.EventService.Add<OnSceneUnloadedEvent>(OnSceneUnloadedHandler);
    }

    private RepairsMinigame GetCurrentMinigame()
    {
        return minigameContainer.Elements
            .FirstOrDefault(mg => mg != null && mg.ShipDamageType == ShipsManager.ShipUnderRepair.DamageType);
    }

    public void StartMinigame()
    {
        if (ShipsManager.ShipUnderRepair == null)
            throw new Exception("Cannot start minigame because the ShipsManager.ShipUnderRepair is null");

        var minigame = GetCurrentMinigame();

        if (minigame == null)
            throw new Exception("Minigame not found in container with type: " + minigame.RepairsMinigameType);

        Debug.Log("Starting minigame with type: " + minigame.RepairsMinigameType);

        SceneLoadingManager.Instance.LoadSceneAsync(minigame.Scene, loadSceneMode: LoadSceneMode.Additive);
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

    public void StopMinigame()
    {
        var minigame = GetCurrentMinigame();

        if (minigame != null)
            SceneLoadingManager.Instance.UnloadSceneAsync(minigame.Scene);
    }

    private void OnSceneLoadedHandler(OnSceneLoadedEvent loadedEvent)
    {
        if (!loadedEvent.IsRepairsMinigameScene)
            return;

        var minigameBehaviour = FindObjectOfType<RepairsMinigameBehaviour>();

        if (minigameBehaviour != null)
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