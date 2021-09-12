using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UICanvasType
{
    None, Terminal, Vending, Hangar, Cassette, NoticeBoard, MainMenu, Bed
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private UICanvasBase bedCanvas;
    [SerializeField] private UICanvasBase terminalCanvas;
    [SerializeField] private UICanvasBase vendingCanvas;
    [SerializeField] private UICanvasBase hangarNodeCanvas;
    [SerializeField] private UICanvasBase casetteCanvas;
    [SerializeField] private UICanvasBase noticeBoardCanvas;
    [SerializeField] private UICanvasBase mainMenuCanvas;

    /// <summary>
    /// Keys that cannot be used for regular UI input until the override is lifted.
    /// e.g. Escape key when in a submenu. 
    /// </summary>
    private static HashSet<KeyCode> currentlyOverriddenKeys;

    private TextMeshPro interactionTextMesh;
    private static UICanvasType currentCanvasType;

    private static bool IsPointerOverButton => UIUtils.IsPointerOverObjectType(typeof(Button));

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
    }

    private void Start()
    {
        ClearCanvases();
        Init();
    }

    public void Init()
    {
        if (DataUtils.IsNewGame())
        {
            //// Show the main menu canvas for character creation
            //SetCanInteract(UICanvasType.MainMenu);
            //ShowCanvas();
        }
    }

    private void Update()
    {
        if (!PlayerManager.IsPaused
            && currentCanvasType != UICanvasType.None
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            ShowCanvas(currentCanvasType);
        }
        // Don't clear canvases if there is KeyCode override in place
        else if (GetNonOverriddenKeyDown(PlayerConstants.ExitKey))
        {
            ClearCanvases();
        }
        // Play an Error sound effect if a non-interactable region is clicked
        else if (PlayerManager.IsPaused
            && Input.GetMouseButtonDown(0)
            && !IsPointerOverButton)
        {
            UISoundEffectsManager.Instance.PlaySoundEffect(UISoundEffect.Error);
        }

        SetInteractionTextMesh();
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
        PlayerManager.IsPaused = false;
        Instance.bedCanvas.SetActive(false);
        Instance.terminalCanvas.SetActive(false);
        Instance.hangarNodeCanvas.SetActive(false);
        Instance.vendingCanvas.SetActive(false);
        Instance.casetteCanvas.SetActive(false);
        Instance.noticeBoardCanvas.SetActive(false);
        Instance.mainMenuCanvas.SetActive(false);

        OnCanvasDeactivated?.Invoke();
    }

    /// <param name="canvasType">The type of canvas to display, which is set by collision or a shortcut
    /// </param>
    /// <param name="viaShortcut">For shortcut access. Will not alter player prefs. 
    /// </param>
    public static void ShowCanvas(UICanvasType canvasType, bool viaShortcut = false)
    {
        ClearCanvases();
        PlayerManager.Instance.EnterMenuState();
        UICanvasBase canvas = GetCanvasByType(canvasType);
        canvas.SetActive(true);

        // Show tutorial overlay if first time using the UI 
        if (!viaShortcut && !CurrentCanvasHasBeenViewed())
        {
            canvas.ShowTutorial();
        }

        OnCanvasActivated?.Invoke();
    }

    private static UICanvasBase GetCanvasByType(UICanvasType canvasType)
    {
        switch (canvasType)
        {
            case UICanvasType.Terminal:
                return Instance.terminalCanvas;
            case UICanvasType.Hangar:
                return Instance.hangarNodeCanvas;
            case UICanvasType.Vending:
                return Instance.vendingCanvas;
            case UICanvasType.Cassette:
                return Instance.casetteCanvas;
            case UICanvasType.NoticeBoard:
                return Instance.noticeBoardCanvas;
            case UICanvasType.Bed:
                return Instance.bedCanvas;
            case UICanvasType.MainMenu:
                return Instance.mainMenuCanvas;
            default:
                Debug.LogError("Invalid UI type passed to GetCanvasByType");
                return null;
        }
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
        currentCanvasType = UICanvasType.None;
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
    private static bool CurrentCanvasHasBeenViewed()
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
}
