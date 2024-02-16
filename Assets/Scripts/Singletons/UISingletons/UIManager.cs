using Events;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private UniversalUI universalUI;
    #endregion

    [SerializeField] private TransitionUI transitionUI;
    public static bool IsTransitioning => Instance.transitionUI.IsTransitioning;

    [SerializeField]
    private List<CanvasAccessSettings> accessSettings = new();

    /// <summary>
    /// Keys that cannot be used for regular UI input until the override is lifted.
    /// e.g. Escape key when in a submenu. 
    /// </summary>
    private static HashSet<KeyCode> currentlyOverriddenKeys;

    private TextMeshPro interactionTextMesh;
    public static UICanvasType CurrentCanvasType { get; private set; }

    private static Type[] SuccessInputTypes => new[]
    {
        typeof(Button), typeof(Slider), typeof(AudioVolumeSlider), typeof(Scrollbar), typeof(MissionUIItem), typeof(PilotInMissionScheduleSlot), typeof(NewDayReportCard),
    };
    private static string[] SuccessInputTags = new[] { MissionConstants.MissionsListRaycastTag };
    private static bool IsErrorInput => Input.GetMouseButtonDown(0)
        && CurrentCanvasType != UICanvasType.Bed
        && !DialogueUtils.IsConversationActive
        && TypesUnderPointer != null
        && !TypesUnderPointer.Any(t => SuccessInputTypes.Contains(t))
        && TagsUnderPointer != null
        && !TagsUnderPointer.Any(t => SuccessInputTags.Contains(t)); 

    public static HashSet<Type> TypesUnderPointer { get; private set; }
    public static HashSet<string> TagsUnderPointer { get; private set; }

    public static TerminalUIManager TerminalManager => Instance.terminalCanvas as TerminalUIManager;
    public static int HangarNode;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

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

        DialogueManager.Instance.conversationStarted += OnConversationStartedHandler;
        DialogueManager.Instance.conversationEnded += OnConversationEndedHandler;
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
        if (CurrentCanvasType != UICanvasType.None && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            UICanvasBase canvas = GetCanvasByType(CurrentCanvasType);

            if (canvas.ZoomInBeforeOpening && canvas.CameraZoomSettings != null)
            {
                StationCameraManager.Instance.ZoomInLiveCamera(canvas.CameraZoomSettings, () => ShowCanvas(canvas));
            }
            else
            {
                ShowCanvas(canvas);
            }
        }

        if (GetNonOverriddenKeyDown(PlayerConstants.PauseKey) && !StationCameraManager.IsLiveCameraZooming)
        {
            CurrentCanvasType = UICanvasType.PauseMenu;
            ShowCanvas(CurrentCanvasType);
        }
    }

    private void HandlePausedInput()
    {
        if (CurrentCanvasType == UICanvasType.None)
        {
            return;
        }

        TypesUnderPointer = UIUtils.GetAllUnderPointer(out var tagsUnderPointer);
        TagsUnderPointer = tagsUnderPointer;

        // Play an Error sound effect if a non-interactable region is clicked
        if (IsErrorInput)
        {
            UISoundEffectsManager.Instance.PlaySoundEffect(UISoundEffect.Error);
            return;
        }

        // Don't clear canvases if there is KeyCode override in place, e.g. in Submenus
        if (GetNonOverriddenKeyDown(PlayerConstants.ExitKey) && !StationCameraManager.IsLiveCameraZooming)
        {
            ClearCanvases();
            CurrentCanvasType = UICanvasType.None;
        }
    }

    public static void ClearCanvases(bool unpausePlayer = true)
    {
        if (unpausePlayer)
        {
            PlayerManager.ExitPausedState();
        }
        Instance.bedCanvas.SetActive(false);
        Instance.terminalCanvas.SetActive(false);
        Instance.hangarNodeCanvas.SetActive(false);
        Instance.vendingCanvas.SetActive(false);
        Instance.casetteCanvas.SetActive(false);
        Instance.noticeBoardCanvas.SetActive(false);
        Instance.mainMenuCanvas.SetActive(false);
        Instance.pauseMenuCanvas.SetActive(false);
        Instance.universalUI.gameObject.SetActive(false);
    }

    /// <param name="canvas">The canvas to display, which is set by collision or a shortcut
    /// </param>
    /// <param name="viaShortcut">For shortcut access. Will not alter player prefs. 
    /// </param>
    public static void ShowCanvas(UICanvasBase canvas, bool viaShortcut = false)
    {
        ClearCanvases();
        PlayerManager.EnterPausedState();

        if (!Instance.IsCanvasAccessible(canvas.CanvasType, out CanvasAccessSettings setting))
        {
            PopupManager.ShowPopup(bodyText: setting.Text);
            return;
        }

        canvas.SetActive(true);

        if (!viaShortcut)
        {
            canvas.ShowTutorialIfExistsAndUnseen();
        }

        if (canvas.ShowUniversalCanvas)
        {
            Instance.universalUI.gameObject.SetActive(true);
        }
    }

    public static void ShowCanvas(UICanvasType canvasType, bool viaShortcut = false)
    {
        UICanvasBase canvas = GetCanvasByType(canvasType);
        ShowCanvas(canvas, viaShortcut);
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
            ShowCanvas(canvas, true);
        }
        else
        {
            ClearCanvases();
        }
    }

    public static bool IsTutorialActive(UICanvasType type)
    {
        var canvas = GetCanvasByType(type);
        return canvas.IsTutorialActive;
    }

    public static void BeginTransition(TransitionUI.TransitionType transitionType, string textContent)
    {
        Instance.transitionUI.BeginTransition(transitionType, textContent);
    }

    private bool IsCanvasAccessible(UICanvasType type, out CanvasAccessSettings setting)
    {
        setting = accessSettings.FirstOrDefault(s => s.CanvasType == type);
        return setting == null || DialogueDatabaseManager.GetLuaVariableAsBool(setting.DialogueVariableName);
    }

    public static void LiftAccessSettings()
    {
        Instance.accessSettings.ForEach(a =>
        {
            DialogueDatabaseManager.Instance.UpdateDatabaseVariable(a.DialogueVariableName, true);
        });
    }

    #region Interaction
    public static void SetCanInteract(UICanvasType canvasType, int node = -1)
    {
        CurrentCanvasType = canvasType;

        // Determines which node we're viewing 
        if (canvasType == UICanvasType.Hangar && HangarManager.NodeIsValid(node))
        {
            HangarNode = node;
        }
    }

    public static void SetCannotInteract()
    {
        if (CurrentCanvasType != UICanvasType.None)
        {
            CurrentCanvasType = UICanvasType.None;
        }
        HangarNode = -1;
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

    #region Event Handlers
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
        if (startedEvent.Cutscene.ConversationSettings.CloseDialogueUIOnStart)
        {
            Debug.Log("Closing dialogue UI on cutscene start...");
            DialogueManager.DialogueUI.Close();
        }

        if (!startedEvent.Cutscene.ConversationSettings.DontPauseDialogueOnStart)
        {
            Debug.Log("Pausing Dialogue System...");
            DialogueManager.Instance.Pause();
        }
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent finishedEvent)
    {
        if (finishedEvent.Cutscene.ConversationSettings.OpenDialogueUIOnEnd)
        {
            Debug.Log("Opening dialogue UI on cutscene end...");
            DialogueManager.DialogueUI.Open();
        }

        if (!finishedEvent.Cutscene.ConversationSettings.DontUnpauseDialogueOnEnd)
        {
            Debug.Log("Unpausing Dialogue System...");
            DialogueManager.Instance.Unpause();
        }

        if (finishedEvent.Cutscene.ConversationSettings.ContinueOnEnd)
        {
            Debug.Log("Continuing to next node on cutscene end...");
            var dialogueUI = FindObjectOfType<AbstractDialogueUI>();
            dialogueUI.OnContinueConversation();
        }
    }

    private void OnConversationStartedHandler(Transform t)
    {
        AddOverriddenKey(KeyCode.Escape);
    }

    private void OnConversationEndedHandler(Transform t)
    {
        RemoveOverriddenKey(KeyCode.Escape);
    }
    #endregion
}
