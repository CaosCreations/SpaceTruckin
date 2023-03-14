using Events;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UICanvasType
{
    None, Terminal, Vending, Hangar, Cassette, NoticeBoard, MainMenu, PauseMenu, Bed
}

/// <summary>
/// Station-specific UI logic.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    #region Canvases
    [SerializeField] private UICanvasBase bedCanvas;
    [SerializeField] private UICanvasBase terminalCanvas;
    [SerializeField] private UICanvasBase vendingCanvas;
    [SerializeField] private UICanvasBase hangarNodeCanvas;
    [SerializeField] private UICanvasBase casetteCanvas;
    [SerializeField] private UICanvasBase noticeBoardCanvas;
    [SerializeField] private UICanvasBase mainMenuCanvas;
    [SerializeField] private UICanvasBase pauseMenuCanvas;
    [SerializeField] private Canvas universalCanvas;
    #endregion

    /// <summary>
    /// Keys that cannot be used for regular UI input until the override is lifted.
    /// e.g. Escape key when in a submenu. 
    /// </summary>
    private static HashSet<KeyCode> currentlyOverriddenKeys;

    private TextMeshPro interactionTextMesh;
    private static UICanvasType currentCanvasType;

    private static bool IsPointerOverButton => UIUtils.IsPointerOverObjectType(typeof(Button));
    private static bool IsErrorInput => Input.GetMouseButtonDown(0)
        && !IsPointerOverButton
        && !DialogueUtils.IsConversationActive
        && currentCanvasType != UICanvasType.Bed;

    public static int HangarNode;

    public static event Action OnCanvasActivated;
    public static event Action OnCanvasDeactivated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentlyOverriddenKeys = new HashSet<KeyCode>();
        interactionTextMesh = GetComponentInChildren<TextMeshPro>();
        interactionTextMesh.text = string.Empty;
    }

    private void Start()
    {
        RegisterEvents();
        ClearCanvases();
    }

    private void RegisterEvents()
    {
        SingletonManager.EventService.Add<OnSceneLoadedEvent>(OnSceneLoadedHandler);
        SingletonManager.EventService.Add<OnSceneUnloadedEvent>(OnSceneUnloadedHandler);

        SingletonManager.EventService.Add<OnCutsceneStartedEvent>(OnCutsceneStartedHandler);
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    private void Update()
    {
        if (!PlayerManager.IsPaused)
        {
            HandleUnpausedInput();
        }
        else
        {
            HandlePausedInput();
        }
    }

    private void HandleUnpausedInput()
    {
        // If we are not in a menu/in range of a UI activator
        if (currentCanvasType != UICanvasType.None)
        {
            if (Input.GetKeyDown(PlayerConstants.ActionKey))
            {
                ShowCanvas(currentCanvasType);
            }
        }
        else if (Input.GetKeyDown(PlayerConstants.PauseKey))
        {
            ShowCanvas(UICanvasType.PauseMenu);
        }
    }

    private void HandlePausedInput()
    {
        // Play an Error sound effect if a non-interactable region is clicked
        if (IsErrorInput)
        {
            UISoundEffectsManager.Instance.PlaySoundEffect(UISoundEffect.Error);
            return;
        }

        // Don't clear canvases if there is KeyCode override in place, e.g. in Submenus
        if (GetNonOverriddenKeyDown(PlayerConstants.ExitKey))
        {
            ClearCanvases();
        }
    }

    private void SetInteractionTextMesh()
    {
        if (currentCanvasType != UICanvasType.None)
        {
            interactionTextMesh.gameObject.SetActive(true);
            interactionTextMesh.SetText(GetInteractionString());

            interactionTextMesh.transform.position =
                PlayerManager.PlayerMovement.transform.position + new Vector3(0, 0.5f, 0);
        }
        else
        {
            interactionTextMesh.gameObject.SetActive(false);
        }
    }

    public static void ClearCanvases()
    {
        PlayerManager.ExitPausedState();
        OnCanvasDeactivated?.Invoke();

        if (Instance != null)
        {
            Instance.bedCanvas.SetActive(false);
            Instance.terminalCanvas.SetActive(false);
            Instance.hangarNodeCanvas.SetActive(false);
            Instance.vendingCanvas.SetActive(false);
            Instance.casetteCanvas.SetActive(false);
            Instance.noticeBoardCanvas.SetActive(false);
            Instance.mainMenuCanvas.SetActive(false);
            Instance.pauseMenuCanvas.SetActive(false);
            Instance.universalCanvas.gameObject.SetActive(false);
        }
    }

    /// <param name="canvasType">The type of canvas to display, which is set by collision or a shortcut
    /// </param>
    /// <param name="viaShortcut">For shortcut access. Will not alter player prefs. 
    /// </param>
    public static void ShowCanvas(UICanvasType canvasType, bool viaShortcut = false)
    {
        ClearCanvases();
        PlayerManager.EnterPausedState();
        UICanvasBase canvas = GetCanvasByType(canvasType);
        canvas.SetActive(true);

        // Show tutorial overlay if first time using the UI 
        if (!viaShortcut && canvas.CanvasTutorialPrefab != null && !HasCurrentCanvasBeenViewed())
        {
            canvas.ShowTutorial();
        }

        // Also show universal canvas if configured 
        if (canvas.ShowUniversalCanvas)
        {
            Instance.universalCanvas.gameObject.SetActive(true);
        }

        OnCanvasActivated?.Invoke();
    }

    private static UICanvasBase GetCanvasByType(UICanvasType canvasType)
    {
        return canvasType switch
        {
            UICanvasType.Terminal => Instance.terminalCanvas,
            UICanvasType.Hangar => Instance.hangarNodeCanvas,
            UICanvasType.Vending => Instance.vendingCanvas,
            UICanvasType.Cassette => Instance.casetteCanvas,
            UICanvasType.NoticeBoard => Instance.noticeBoardCanvas,
            UICanvasType.Bed => Instance.bedCanvas,
            UICanvasType.MainMenu => Instance.mainMenuCanvas,
            UICanvasType.PauseMenu => Instance.pauseMenuCanvas,
            _ => null,
        };
    }

    public static bool IsCanvasActive(UICanvasType canvasType)
    {
        UICanvasBase canvas = GetCanvasByType(canvasType);
        return canvas != null && canvas.IsActive();
    }

    public static void ToggleCanvas(UICanvasType canvasType)
    {
        UICanvasBase canvas = GetCanvasByType(canvasType);
        if (canvas == null)
        {
            Debug.LogError($"UICanvasBase of type '{nameof(canvasType)}' not found");
            return;
        }

        if (!canvas.IsActive())
        {
            ShowCanvas(canvasType, true);
        }
        else
        {
            ClearCanvases();
        }
    }

    #region Interaction
    public static void SetCanInteract(UICanvasType canvasType, int node = -1)
    {
        currentCanvasType = canvasType;

        // We pass in a node value when opening the hangar UI
        if (canvasType == UICanvasType.Hangar && HangarManager.NodeIsValid(node))
        {
            HangarNode = node;
        }
    }

    public static void SetCannotInteract()
    {
        if (currentCanvasType != UICanvasType.None)
        {
            currentCanvasType = UICanvasType.None;
        }
        HangarNode = -1;
    }

    private static string GetInteractionString()
    {
        string interaction = $"Press {PlayerConstants.ActionKey} to ";
        switch (currentCanvasType)
        {
            case UICanvasType.Bed:
                interaction += "Sleep";
                break;
            case UICanvasType.Cassette:
                interaction += "Play Music";
                break;
            case UICanvasType.Hangar:
                interaction += "Manage Ship";
                break;
            case UICanvasType.NoticeBoard:
                interaction += "Accept Missions";
                break;
            case UICanvasType.Terminal:
                interaction += "Manage Company";
                break;
            case UICanvasType.Vending:
                interaction += "Buy Snax";
                break;
            case UICanvasType.MainMenu:
            default:
                return string.Empty;
        }

        return interaction;
    }
    #endregion

    #region PlayerPrefs
    private static bool HasCurrentCanvasBeenViewed()
    {
        return PlayerPrefsManager.GetHasBeenViewedPref(currentCanvasType);
    }

    public static void SetCurrentCanvasHasBeenViewed(bool value)
    {
        PlayerPrefsManager.SetHasBeenViewedPref(currentCanvasType, value);
    }
    #endregion

    #region KeyOverriding
    /// <summary>
    /// Returns true if the key is down and is not being overridden by another menu.
    /// </summary>
    /// <param name="keyCode"></param>
    public static bool GetNonOverriddenKeyDown(KeyCode keyCode)
    {
        return Input.GetKeyDown(keyCode) && !currentlyOverriddenKeys.Contains(keyCode);
    }

    public static void AddOverriddenKeys(HashSet<KeyCodeOverride> keyCodeOverrides)
    {
        keyCodeOverrides ??= new HashSet<KeyCodeOverride>();
        currentlyOverriddenKeys ??= new HashSet<KeyCode>();

        var keyCodes = keyCodeOverrides.ToListOfKeyCodes();
        currentlyOverriddenKeys.UnionWith(keyCodes);
    }

    public static void AddOverriddenKey(KeyCode keyCode)
    {
        if (!currentlyOverriddenKeys.Contains(keyCode))
        {
            currentlyOverriddenKeys.Add(keyCode);
        }
    }

    public static void RemoveOverriddenKeys(HashSet<KeyCodeOverride> keyCodeOverrides)
    {
        // Don't remove the persistent overridden keycodes from the list 
        var nonPersistentKeyCodes = keyCodeOverrides.ToListOfNonPersistentKeyCodes();

        currentlyOverriddenKeys.ExceptWith(nonPersistentKeyCodes);
    }

    public static void RemoveOverriddenKey(KeyCode keyCode)
    {
        if (currentlyOverriddenKeys.Contains(keyCode))
        {
            currentlyOverriddenKeys.Remove(keyCode);
        }
    }

    public static void ResetOverriddenKeys()
    {
        currentlyOverriddenKeys.Clear();
    }
    #endregion

    private void OnSceneLoadedHandler(OnSceneLoadedEvent loadedEvent)
    {
        // Override Escape closing the UI canvas if we are in a repairs minigame scene 
        if (loadedEvent.IsRepairsMinigameScene)
            AddOverriddenKey(KeyCode.Escape);
    }

    private void OnSceneUnloadedHandler(OnSceneUnloadedEvent unloadedEvent)
    {
        if (unloadedEvent.IsRepairsMinigameScene)
            RemoveOverriddenKey(KeyCode.Escape);
    }

    private void OnCutsceneStartedHandler(OnCutsceneStartedEvent startedEvent)
    {
        if (startedEvent.Cutscene.IsDialogueCutscene)
        {
            Debug.Log("Dialogue cutscene started event call back fired. Closing dialogue UI...");
            DialogueManager.DialogueUI.Close();
        }
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent finishedEvent)
    {
        if (finishedEvent.Cutscene.IsDialogueCutscene)
        {
            Debug.Log("Dialogue cutscene finished event call back fired. Opening dialogue UI...");
            DialogueManager.DialogueUI.Open();
        }
    }
}
